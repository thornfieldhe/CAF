using System;
using System.Linq;
using System.Text;

namespace CAF.Model
{
    using CAF.Ext;
    using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Linq;

    using Fluentx;

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
            var b = Encoding.UTF8.GetBytes(document);
            var xele = XElement.Load(XmlReader.Create(new MemoryStream(b)));
            var modelId = xele.Attribute(XName.Get("UniqueID")).Value.ToGuid();
            var workflow = WorkflowProcess.Get(modelId);
            workflow.IfNull(() => workflow = new WorkflowProcess() { Id = modelId });
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

        #region 工作流实例操作

        /// <summary>
        /// 获取工作流模版
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal WorkflowProcess GetWorkflow(Guid id) { return WorkflowProcess.Get(id); }

        /// <summary>
        /// 创建流程
        /// </summary>
        /// <param name="moduleId">业务类Id</param>
        /// <param name="modelName">工作流模块名称</param>
        /// <param name="userInfo">创建者</param>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <param name="conditions">流程条件</param>
        internal WorkflowState CreateWorkflow(Guid moduleId, string modelName, User userInfo
            , IDbConnection conn, IDbTransaction transaction, params string[] conditions)
        {
            var _process = WorkflowProcess.Get(modelName);
            if (_process != null)
            {
                var process = new WfProcess()
                                      {
                                          Name = modelName,
                                          ModulelId = moduleId,
                                          Document = _process.Document,
                                          Status = (int)WorkflowState.InAudit,
                                          CreatedBy = userInfo.Id,
                                          ModifyBy = userInfo.Id,
                                          OrganizeId = userInfo.OrganizeId,
                                          WfActivitys = new WfActivityList(),
                                          WfRules = new WfRuleList()
                                      };
                _process.WorkflowActivitys.ForEach(a =>
                    {
                        process.WfActivitys.Add(new WfActivity
                                                    {
                                                        WfProcessId = process.Id,
                                                        ActivityId = a.Id,
                                                        Name = a.Name,
                                                        Post = a.Post,
                                                        Type = a.Type,
                                                        Status = (int)WorkflowState.Other
                                                    });
                    });
                _process.WorkflowRules.ForEach(r =>
                    {
                        process.WfRules.Add(new WfRule
                                                {
                                                    RuleId = r.Id,
                                                    BeginActivityID = r.BeginActivityID,
                                                    Condition = r.Condition,
                                                    EndActivityID = r.EndActivityID,
                                                    Name = r.Name,
                                                    Type = r.Type,
                                                    WfProcessId = process.Id
                                                });
                    });


                process.Insert(conn, transaction);
                var initalActivity = process.WfActivitys.Single(a => a.Type == ActivityType.INITIAL.ToString());
                this.AddAudit(userInfo, initalActivity, "流程发起", conn, transaction);
                var result = this.ApprovalPassed(t, initalActivity, null, conditions);
                transaction.Commit();
                return result;

            }
            return WorkflowState.Passed;
        }

        /// <summary>
        /// 创建流程（如果已经存在，删除现有流程再创建）
        /// </summary>
        /// <param name="t"></param>
        /// <param name="modelName"></param>
        /// <param name="userInfo"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        internal WorkflowState ReCreateWorkflow(IBusinessBase t, string modelName, User userInfo, params string[] conditions)
        {
            using (var conn = SqlService.Instance.Connection)
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    var workflows = WfProcess.GetAllByModuleId(t.Id);
                    workflows.ForEach(w =>
                        {
                            w.WfActivitys.ForEach(wa => wa.MarkDelete());
                            w.WfRules.ForEach(wr => wr.MarkDelete());
                            w.MarkDelete();
                            var initalActivity = w.WfActivitys.Single(a => a.Type == ActivityType.INITIAL.ToString());
                            this.AddAudit(userInfo, initalActivity, "流程撤销", conn, transaction);
                        });
                    workflows.SaveChanges(conn, transaction);
                    return this.CreateWorkflow(t, modelName, userInfo, conn, transaction, conditions);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

            }
        }

        /// <summary>
        /// 提交审核
        /// </summary>
        /// <param name="t">实现了IBusinessBase接口的业务表单类</param>
        /// <param name="activity">工作流活动</param>
        /// <param name="reviewId">回退活动ID</param>
        /// <param name="state">审核状态</param>
        /// <param name="conditions">自定义条件</param>
        /// <returns>审核状态</returns>
        internal int SendApproval(IBusinessBase t, WfActivity activity, Guid? reviewId, WorkflowState state, params string[] conditions)
        {
            if (activity != null)
            {
                //更新当前活动状态
                activity.Status = (int)state;
                DataContent.SubmitChanges();
                //查找后续节点
                switch (state)
                {
                    #region 通过

                    case ActivityState.Pass:
                        List<WorkFlowRule> nextRules = DataContent.WorkFlowRule.
                            Where(p => p.WorkFlowID == activity.WorkFlowID && p.BeginActivityID == activity.ActivityID
                                ).ToList();
                        //更新后续节点状态
                        //typeof(t).Name;
                        DataTable dt = Common.ListToDataTable(t);
                        bool hasNextActivity = false;//是否存在下一步
                        foreach (WorkFlowRule rule in nextRules)
                        {
                            string newCondition = "";
                            if (enableCondition)
                            {
                                Regex re = new Regex(@"{\d+}");
                                int rCount = re.Matches(rule.Condition).Count;

                                if (conditions != null && rCount > 0 && rCount <= conditions.Length)
                                {
                                    newCondition = string.Format(rule.Condition, conditions);
                                }
                            }
                            else
                            {
                                newCondition = rule.Condition;
                            }
                            if (string.IsNullOrWhiteSpace(newCondition))
                            {
                                newCondition = "1=1";
                            }
                            //DataTable dtbl = DbHelperSQL.Query(string.Format("select * from {0} where {1} and ID='{2}'", ((Type)t.GetType()).Name, newCondition, t.ID)).Tables[0];//改这个
                            DataRow[] dr = dt.Select(newCondition);
                            if (dr.Length > 0)
                            {
                                #region 与分支要判断到本分支的所有节点都已完成

                                if (rule.WorkFlowEnd.ActivityType == ActivityType.AND_MERGE.ToString())
                                {
                                    bool allowpass = true;
                                    foreach (WorkFlowRule r in rule.WorkFlowEnd.WorkFlowRuleEnd)
                                    {
                                        if (r.WorkFlowBegin.State != (int)ActivityState.Pass)
                                        {
                                            allowpass = false;
                                        }
                                    }
                                    if (allowpass)//true 本节点pass，下一节点process
                                    {
                                        //rule.WorkFlowEnd.State = (int)FormState.Pass;
                                        //DataContent.SubmitChanges();
                                        this.SendApproval(t, activity, null, ActivityState.Pass, false, conditions);
                                    }
                                    else
                                    {
                                        DataContent.SubmitChanges();
                                    }
                                }

                                #endregion 与分支要判断到本分支的所有节点都已完成

                                #region 或分支要判断本分支是否已经完成，若完成则终止，不进行下一步，否则将下一步也完成

                                else if (rule.WorkFlowEnd.ActivityType == ActivityType.OR_MERGE.ToString()
                                    && rule.WorkFlowEnd.State == (int)ActivityState.Other)
                                {
                                    //DataContent.SubmitChanges();
                                    return this.SendApproval(t, rule.WorkFlowEnd, null, ActivityState.Pass, false, conditions);
                                }

                                #endregion 或分支要判断本分支是否已经完成，若完成则终止，不进行下一步，否则将下一步也完成

                                #region 结束

                                else if (rule.WorkFlowEnd.ActivityType == ActivityType.COMPLETION.ToString())
                                {
                                    rule.WorkFlowEnd.State = (int)ActivityState.Pass;
                                    rule.WorkFlowBegin.WorkFlowProcess.State = (int)ActivityState.Pass;
                                    DataContent.SubmitChanges();
                                    AddAudit("系统", rule.WorkFlowEnd, "流程结束", FormState.Other);
                                    return (int)FormState.Pass;
                                }

                                #endregion 结束

                                #region 处理

                                else
                                {
                                    if (!HasAuditPersonInThisActivity(rule.WorkFlowEnd))//下一步如果没有人则跳过
                                    {
                                        //rule.WorkFlowEnd.State = (int)ActivityState.Process;
                                        //DataContent.SubmitChanges();
                                        AddAudit("系统", rule.WorkFlowEnd, "该节点无处理人，流程自动跳过", FormState.Other);
                                        return this.SendApproval(t, rule.WorkFlowEnd, null, ActivityState.Pass, false, conditions);
                                    }
                                    else
                                    {
                                        rule.WorkFlowEnd.State = (int)ActivityState.Process;
                                        DataContent.SubmitChanges();
                                        //return (int)ActivityState.Process;
                                    }
                                }

                                #endregion 处理

                                hasNextActivity = true;
                            }
                        }
                        if (!hasNextActivity)
                        {
                            activity.WorkFlowProcess.State = (int)ActivityState.Pass;
                            DataContent.SubmitChanges();
                            AddAudit("系统", activity, "找不到下一步，流程自动结束", FormState.Other);
                            return (int)FormState.Pass;
                        }
                        break;

                    #endregion 通过

                    #region 退回

                    case ActivityState.Cancel:

                        ///初始化退回活动状态
                        ///如果退回活动类型为INITIAL
                        ///则更新状态为Cancel,否则更新为Review

                        activity.State = (int)ActivityState.Cancel;
                        AddAudit("系统", activity, "流程撤销", FormState.Other);
                        //判断是否是退回、撤销
                        WorkFlowActivity initialActivity = DataContent.WorkFlowActivity.SingleOrDefault(p => reviewId.HasValue
                            && p.ActivityType == ActivityType.INITIAL.ToString() && p.State == (int)ActivityState.Pass
                            && p.WorkFlowID == activity.WorkFlowID && p.ActivityID == reviewId.Value);
                        if (initialActivity != null)
                        {
                            initialActivity.State = (int)ActivityState.Process;
                            initialActivity.WorkFlowProcess.State = (int)FormState.Cancel;
                            DataContent.SubmitChanges();
                            return (int)FormState.Cancel;
                        }
                        else//判断是否是回退至中间步骤
                        {
                            //AuditInfo message = DataContent.AuditInfo.SingleOrDefault(m => m.WorkFlowID == activity.WorkFlowID
                            //            && m.ActivityID == activity.ActivityID && m.State == (int)FormState.Process);
                            // if (message != null)
                            // {
                            // message.State = (int)FormState.Deleted;
                            // }
                            // DataContent.SubmitChanges();
                            WorkFlowActivity reviewActivity = DataContent.WorkFlowActivity.SingleOrDefault(r => r.ActivityID == reviewId.Value
                                && r.WorkFlowID == activity.WorkFlowID);
                            if (reviewActivity != null)
                            {
                                ReSetNextActivitieStates(reviewActivity);
                            }
                        }
                        break;

                    #endregion 退回

                    default:
                        break;
                }
                return (int)FormState.Process;
            }
            return (int)FormState.Pass;
        }


        internal WorkflowState ApprovalPassed(IBusinessBase t, WfActivity activity, Guid? reviewId
            , IDbConnection conn, IDbTransaction transaction,  params string[] conditions)
        {
            if (activity!=null)
            {
                activity.Status = (int)WorkflowState.Passed;

                        WfRule[] nextRules =new WfRuleList().Query<WfRuleList,WfRule>(conn,transaction
                            ,new{WfProcessId=activity.WfProcessId,BeginActivityID=activity.ActivityId}
                            ," WfProcessId=@WfProcessId And BeginActivityID=@BeginActivityID"
                            ).Members;
                        //更新后续节点状态
                        //typeof(t).Name;
                        DataTable dt = Common.ListToDataTable(t);
                
                        bool hasNextActivity = false;//是否存在下一步
                        foreach (WorkFlowRule rule in nextRules)
                        {
                            string newCondition = "";
                            if (enableCondition)
                            {
                                Regex re = new Regex(@"{\d+}");
                                int rCount = re.Matches(rule.Condition).Count;

                                if (conditions != null && rCount > 0 && rCount <= conditions.Length)
                                {
                                    newCondition = string.Format(rule.Condition, conditions);
                                }
                            }
                            else
                            {
                                newCondition = rule.Condition;
                            }
                            if (string.IsNullOrWhiteSpace(newCondition))
                            {
                                newCondition = "1=1";
                            }
                            //DataTable dtbl = DbHelperSQL.Query(string.Format("select * from {0} where {1} and ID='{2}'", ((Type)t.GetType()).Name, newCondition, t.ID)).Tables[0];//改这个
                            DataRow[] dr = dt.Select(newCondition);
                            if (dr.Length > 0)
                            {
                                #region 与分支要判断到本分支的所有节点都已完成

                                if (rule.WorkFlowEnd.ActivityType == ActivityType.AND_MERGE.ToString())
                                {
                                    bool allowpass = true;
                                    foreach (WorkFlowRule r in rule.WorkFlowEnd.WorkFlowRuleEnd)
                                    {
                                        if (r.WorkFlowBegin.State != (int)ActivityState.Pass)
                                        {
                                            allowpass = false;
                                        }
                                    }
                                    if (allowpass)//true 本节点pass，下一节点process
                                    {
                                        //rule.WorkFlowEnd.State = (int)FormState.Pass;
                                        //DataContent.SubmitChanges();
                                        this.SendApproval(t, activity, null, ActivityState.Pass, false, conditions);
                                    }
                                    else
                                    {
                                        DataContent.SubmitChanges();
                                    }
                                }

                                #endregion 与分支要判断到本分支的所有节点都已完成

                                #region 或分支要判断本分支是否已经完成，若完成则终止，不进行下一步，否则将下一步也完成

                                else if (rule.WorkFlowEnd.ActivityType == ActivityType.OR_MERGE.ToString()
                                    && rule.WorkFlowEnd.State == (int)ActivityState.Other)
                                {
                                    //DataContent.SubmitChanges();
                                    return this.SendApproval(t, rule.WorkFlowEnd, null, ActivityState.Pass, false, conditions);
                                }

                                #endregion 或分支要判断本分支是否已经完成，若完成则终止，不进行下一步，否则将下一步也完成

                                #region 结束

                                else if (rule.WorkFlowEnd.ActivityType == ActivityType.COMPLETION.ToString())
                                {
                                    rule.WorkFlowEnd.State = (int)ActivityState.Pass;
                                    rule.WorkFlowBegin.WorkFlowProcess.State = (int)ActivityState.Pass;
                                    DataContent.SubmitChanges();
                                    AddAudit("系统", rule.WorkFlowEnd, "流程结束", FormState.Other);
                                    return (int)FormState.Pass;
                                }

                                #endregion 结束

                                #region 处理

                                else
                                {
                                    if (!HasAuditPersonInThisActivity(rule.WorkFlowEnd))//下一步如果没有人则跳过
                                    {
                                        //rule.WorkFlowEnd.State = (int)ActivityState.Process;
                                        //DataContent.SubmitChanges();
                                        AddAudit("系统", rule.WorkFlowEnd, "该节点无处理人，流程自动跳过", FormState.Other);
                                        return this.SendApproval(t, rule.WorkFlowEnd, null, ActivityState.Pass, false, conditions);
                                    }
                                    else
                                    {
                                        rule.WorkFlowEnd.State = (int)ActivityState.Process;
                                        DataContent.SubmitChanges();
                                        //return (int)ActivityState.Process;
                                    }
                                }

                                #endregion 处理

                                hasNextActivity = true;
                            }
                        }
                        if (!hasNextActivity)
                        {
                            activity.WorkFlowProcess.State = (int)ActivityState.Pass;
                            DataContent.SubmitChanges();
                            AddAudit("系统", activity, "找不到下一步，流程自动结束", FormState.Other);
                            return (int)FormState.Pass;
                        }

            }
        }

        /// <summary>
        /// 创建审核意见
        /// </summary>
        /// <param name="user"></param>
        /// <param name="activity">审核活动</param>
        /// <param name="idea">审核意见</param>
        /// <param name="con"></param>
        /// <param name="tran"></param>
        /// <param name="messageState">审核状态</param>
        internal void AddAudit(User user, WfActivity activity, string idea
            , IDbConnection con, IDbTransaction tran
            , WorkflowState messageState = WorkflowState.Passed)
        {
            var audit = new WfAuditOption()
            {
                WfActivityId = activity.ActivityId,
                AuditOpinion = idea,
                AuditName = user.Name,
                PostName = activity.Post.HasValue ? "" : Post.Get(activity.Post.Value).Name,
                Status = (int)messageState,
                WfProcessId = activity.WfProcessId
            };
            audit.SaveChange(con, tran);
        }

        #endregion
    }
}
