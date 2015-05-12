using System.Web.Services;

namespace CAF.Web
{
    using CAF.Model;
    using System;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Workflow 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Workflow : System.Web.Services.WebService
    {


        /// <summary>
        /// 获取工作流列表XML文件
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetWorkFlowList()
        {
            try
            {
                return Model.Workflow.GetWorkflowsAsXML();
            }
            catch (Exception ex)
            {
                this.CreateErrorLog(ex);
                return "";
            }
        }

        /// <summary>
        /// 获取单条工作流信息
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetWorkflowDocument(string workflowId)
        {
            try
            {
                return Model.Workflow.GetWorkflowAsXML(workflowId.ToGuid());
            }
            catch (Exception ex)
            {
                this.CreateErrorLog(ex);
                return "";
            }
        }


        /// <summary>
        /// 删除工作流
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        [WebMethod]
        public void DeleteWorkflow(string workflowId)
        {
            try
            {
                Model.Workflow.DeleteWorkflow(workflowId.ToGuid());
            }
            catch (Exception ex) { this.CreateErrorLog(ex); }
        }


        /// <summary>
        /// 更新工作流
        /// </summary>
        /// <param name="workflowDocument"></param>
        /// <returns></returns>
        [WebMethod]
        public void UpdateWorkflow(string workflowDocument)
        {
            try
            {
                Model.Workflow.UpdateWorkflow(workflowDocument);
            }
            catch (Exception ex)
            {
                this.CreateErrorLog(ex);
                throw;
            }
        }


        /// <summary>
        /// 获取岗位列表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetPostList()
        {
            try
            {

                var doc = new XDocument(new XElement("WorkFlows", Post.GetAll().Select(p => new XElement("Post",
                                   new XAttribute("postname", p.Name.ToString()),
                                   new XAttribute("ID", p.Id.ToString())))));
                return doc.ToString();
            }
            catch (Exception ex)
            {
                this.CreateErrorLog(ex);
                return "";
            }
        }

        private void CreateErrorLog(Exception ex)
        {
            var log = new ErrorLog()
                          {
                              Details = ex.StackTrace,
                              UserName = this.User.Identity.Name,
                              Ip = Net.GetClientIP(),
                              PageCode = 0,
                              Message = ex.Message,
                              Page = "工作流"
                          };
            log.Create();
        }
    }
}
