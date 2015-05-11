using System;
using System.Linq;
using System.Text;

namespace CAF.Model.Workflow
{
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
            return
                WorkflowProcess.GetAll()
                    .Select(
                        x =>
                        new XElement("Workflow",
                            new XAttribute("Name", string.IsNullOrWhiteSpace(x.Name) ? x.Id.ToString() : x.Name),
                            new XAttribute("Id", x.Id.ToString())))
                    .ToString();
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
            var moduleName = xele.Attribute(XName.Get("Name")).Value;
            var module = Module.Get(moduleName);
            module.WorkflowProcess.Name = xele.Attribute(XName.Get("Name")).Value;
            module.WorkflowProcess.Document = document;
            module.WorkflowProcess.WorkflowActivitys.ForEach(w => w.MarkDelete());
            module.WorkflowProcess.WorkflowRules.ForEach(w => w.MarkDelete());

            var partNos = from item in xele.Descendants("Activity") select item;
            foreach (var node in partNos)
            {

                var activity = new WorkflowActivity
                                   {
                                       Id = new Guid(node.Attribute(XName.Get("UniqueID")).Value),
                                       Name = node.Attribute(XName.Get("ActivityName")).Value,
                                       Type = node.Attribute(XName.Get("Type")).Value
                                   };
                if (!string.IsNullOrEmpty(node.Attribute(XName.Get("ActivityPost")).Value))
                    activity.Post = new Guid(node.Attribute(XName.Get("ActivityPost")).Value);
                module.WorkflowProcess.WorkflowActivitys.Add(activity);
            }

            partNos = from item in xele.Descendants("Rule") select item;

            foreach (var rule in partNos.Select(node => new WorkflowRule
                    {
                        BeginActivityID =
                            new Guid(node.Attribute(XName.Get("BeginActivityUniqueID")).Value),
                        Condition = CharTrans(node.Attribute(XName.Get("RuleCondition")).Value),
                        EndActivityID =
                            new Guid(node.Attribute(XName.Get("EndActivityUniqueID")).Value),
                        Id = new Guid(node.Attribute(XName.Get("UniqueID")).Value),
                        Name = node.Attribute(XName.Get("RuleName")).Value,
                        Type = node.Attribute(XName.Get("LineType")).Value
                    }))
            {
                module.WorkflowProcess.WorkflowRules.Add(rule);
            }
            module.SubmitChange();

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
