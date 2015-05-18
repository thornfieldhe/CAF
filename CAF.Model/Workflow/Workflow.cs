using System;
using System.Linq;
using System.Text;

namespace CAF.Model
{
    using CAF.Ext;
    using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    public partial class Workflow
    {
        #region 工作流模版编辑

        /// <summary>
        /// 获取所有预定义工作流
        /// 生成XML文件
        /// </summary>
        /// <returns></returns>
        public static string GetWorkflowsAsXML()
        {

            return new XDocument(new XElement("WorkFlows",
                WorkflowProcess.GetAll().Select(
                   x =>
                   new XElement("Workflow",
                       new XAttribute("Name", string.IsNullOrWhiteSpace(x.Name) ? x.Id.ToString() : x.Name),
                       new XAttribute("Id", x.Id.ToString())))
               .ToList())).ToString();
        }

        /// <summary>
        /// 获取预定义工作流
        /// XML文件
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static string GetWorkflowAsXML(Guid workflowId)
        {
            return WorkflowProcess.Get(workflowId).Document;
        }

        /// <summary>
        /// 删除预定义工作流
        /// </summary>
        /// <param name="workflowId"></param>
        public static void DeleteWorkflow(Guid workflowId)
        {
            WorkflowProcess.Delete(workflowId);
        }

        /// <summary>
        /// 更新预定义工作流
        /// </summary>
        /// <param name="document"></param>
        public static void UpdateWorkflow(string document)
        {
            var b = UTF8Encoding.UTF8.GetBytes(document);
            var xele = XElement.Load(XmlReader.Create(new MemoryStream(b)));
            var modelId = xele.Attribute(XName.Get("UniqueID")).Value.ToGuid();
            var workflow = WorkflowProcess.Get(modelId);
            workflow.IfNull(() =>
                {
                    workflow = new WorkflowProcess() { };
                    workflow.SetId(modelId);
                });
            workflow.Name = xele.Attribute(XName.Get("Name")).Value;
            workflow.Document = document;
            workflow.WorkflowActivitys.ForEach(w => w.MarkDelete());
            workflow.WorkflowRules.ForEach(w => w.MarkDelete());

            //活动处理
            var newActivities = new WorkflowActivityList();
            xele.Descendants("Activity").ForEach(r =>
                {
                    var activity = new WorkflowActivity
                                       {
                                           Id = new Guid(r.Attribute(XName.Get("UniqueID")).Value),
                                           Name = r.Attribute(XName.Get("ActivityName")).Value,
                                           Type = r.Attribute(XName.Get("Type")).Value
                                       };
                    string.IsNullOrEmpty(r.Attribute(XName.Get("ActivityPost")).Value).IfIsFalse(() =>
                         activity.Post = r.Attribute(XName.Get("ActivityPost")).Value.ToGuid());
                    newActivities.Add(activity);
                });

            var activityDataSet1 = workflow.WorkflowActivitys.Select(i => i.Id).Intersect(newActivities.Select(i => i.Id));//交集
            var activityDataSet2 = workflow.WorkflowActivitys.Select(i => i.Id).Intersect(activityDataSet1);//删除集合
            var activityDataSet3 = newActivities.Select(i => i.Id).Intersect(activityDataSet1);//新增集合
            workflow.WorkflowActivitys.Where(a => activityDataSet2.Contains(a.Id)).ForEach(a => a.MarkDelete());
            workflow.WorkflowActivitys.Where(a => activityDataSet1.Contains(a.Id)).ForEach(a =>
                a = newActivities.Single(n => n.Id == a.Id));
            workflow.WorkflowActivitys.AddRange(newActivities.Where(a => activityDataSet3.Contains(a.Id)).ToList());
            //规则处理
            var newRules = new WorkflowRuleList();
            xele.Descendants("Rule").ForEach(r =>
                {
                    newRules.Add(new WorkflowRule
                                   {
                                       BeginActivityID =
                                              new Guid(r.Attribute(XName.Get("BeginActivityUniqueID")).Value),
                                       Condition = CharTrans(r.Attribute(XName.Get("RuleCondition")).Value),
                                       EndActivityID =
                                           new Guid(r.Attribute(XName.Get("EndActivityUniqueID")).Value),
                                       Id = new Guid(r.Attribute(XName.Get("UniqueID")).Value),
                                       Name = r.Attribute(XName.Get("RuleName")).Value,
                                       Type = r.Attribute(XName.Get("LineType")).Value
                                   });
                });
            var ruleDataSet1 = workflow.WorkflowRules.Select(i => i.Id).Intersect(newRules.Select(i => i.Id));//交集
            var ruleDataSet2 = workflow.WorkflowRules.Select(i => i.Id).Intersect(ruleDataSet1);//删除集合
            var ruleDataSet3 = newRules.Select(i => i.Id).Intersect(ruleDataSet1);//新增集合
            workflow.WorkflowRules.Where(a => ruleDataSet2.Contains(a.Id)).ForEach(a => a.MarkDelete());
            workflow.WorkflowRules.Where(a => ruleDataSet1.Contains(a.Id)).ForEach(a =>
                a = newRules.Single(n => n.Id == a.Id));
            workflow.WorkflowRules.AddRange(newRules.Where(a => ruleDataSet3.Contains(a.Id)).ToList());

            workflow.SubmitChange();

        }

        /// <summary>
        /// 替换xml中尖括号
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string CharTrans(string source)
        {
            if (source.Contains("大于"))
            {
                source = source.Replace("大于", ">");
            }
            if (source.Contains("小于"))
            {
                source = source.Replace("小于", "<");
            }
            if (source.Contains("等于"))
            {
                source = source.Replace("等于", "=");
            }
            return source;
        }

        #endregion

    }
}
