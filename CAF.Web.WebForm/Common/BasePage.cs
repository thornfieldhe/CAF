using FineUI;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;

    using global::System.Linq;

    public class BasePage : Page
    {

        /// <summary>
        /// 模块名称
        /// </summary>
        protected string module;
        /// <summary>
        /// 页面编码
        /// </summary>
        protected Guid pageId;

        protected bool allowSubmit = true;

        /// <summary>
        /// 待编辑项Id
        /// </summary>
        protected Guid Id
        {
            get
            {
                return this.Request["Id"].ToGuid();
            }
        }

        protected List<Guid> readRule;
        protected string writeRule = "";

        public BasePage()
        {
            this.pageId = Guid.NewGuid();
            this.module = string.Empty;
        }



        protected User LoginUser
        {
            get
            {
                return Model.User.Get(this.User.Identity.Name);
            }
        }

        #region 控件初始化

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            this.module = Directory.Get(this.pageId).Name;
            if (this.IsPostBack)
            {
                return;
            }
            try
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var roles = this.LoginUser.Roles.Select(r => r.Id).ToList();
                    var canRead = this.CanRead(roles);
                    var canEdit = this.CanEdit(roles);

                    if (canRead || canEdit)
                    {
                        this.Title = this.module;
                        this.Initialization();
                        if (canEdit)
                        {
                            this.InitializationEditControls();
                        }
                        else
                        {
                            this.InitializationReadControls();
                        }

                        this.CreateInfoLog(Resource.System_Action_LoginIn);
                    }
                    else
                    {
                        this.UnAuthenticated();
                    }
                }
                else
                {
                    this.UnAuthenticated();
                }
            }
            catch (Exception ex)
            {
                this.CreateErrorLog(ex);
            }
        }

        /// <summary>
        /// 初始化工作
        /// 清理控件并绑定
        /// 将自动执行绑定控件工作
        /// </summary>
        protected void Initialization()
        {
            PageTools.ClearControls(this.Page);
            this.PageBind();
        }



        /// <summary>
        /// 具体控件绑定工作
        /// </summary>
        protected virtual void Bind() { }

        protected void PageBind()
        {
            try
            {
                this.Bind();
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
                this.CreateErrorLog(ex);
            }
        }

        /// <summary>
        /// 绑定只读模式下控件的状态
        /// </summary>
        protected virtual void InitializationReadControls()
        {
            this.InitializationReadControls(this.Page);
        }

        /// <summary>
        /// 绑定只读模式下控件的状态
        /// </summary>
        protected virtual void InitializationReadControls(Control ctl)
        {
            if (ctl.HasControls())
            {
                foreach (Control item in ctl.Controls)
                {
                    this.InitializationReadControls(item);
                }
            }
            else
            {

                if (!(ctl is Button))
                {
                    return;
                }
                var button = (Button)ctl;
                if (button.ID.Contains("Add") || button.ID.Contains("Delete") ||
                    button.ID.Contains("Submit") || button.ID.Contains("Edit") ||
                    button.ID.Contains("Save") || button.ID.Contains("Update") ||
                    button.ID.Contains("Reset") || button.ID.Contains("New"))
                {
                    button.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 绑定可写模式下控件的状态
        /// </summary>
        protected virtual void InitializationEditControls() { }

        #endregion 控件初始化

        #region 授权

        protected virtual bool CanRead(List<Guid> roles)
        {
            return
                Directory_Role.GetAllByDirectoryId(this.pageId).Where(r => r.Status == (int)RightStatusEnum.Read)
                .Select(r => r.RoleId).ToList().Intersect(roles).Count() > 0;
        }

        protected virtual bool CanEdit(List<Guid> roles)
        {
            return
                Directory_Role.GetAllByDirectoryId(this.pageId).Where(r => r.Status == (int)RightStatusEnum.Write)
                .Select(r => r.RoleId).ToList().Intersect(roles).Count() > 0;
        }



        protected virtual void UnAuthenticated()
        {
            this.Response.Redirect("~/Login.aspx?referrer=" + this.Server.UrlEncode(this.Request.Url.PathAndQuery));
        }

        #endregion 授权

        #region 按钮事件

        protected virtual void btnExcute_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button)
                {
                    var btn = (Button)sender;
                    switch (btn.ID)
                    {
                        case "btnAdd":
                            this.Add();
                            this.CreateInfoLog(Resource.System_Action_Add);
                            break;
                        case "btnUpdate":
                            this.Update();
                            this.CreateInfoLog(Resource.System_Action_Update);
                            break;
                        case "btnDelete":
                            this.Delete();
                            this.CreateInfoLog(Resource.System_Action_Delete);
                            break;
                        case "btnQuery":
                            this.Query();
                            this.CreateInfoLog(Resource.System_Action_Query);
                            break;
                        case "btnSave":
                            this.Save();
                            this.CreateInfoLog(Resource.System_Action_Save);
                            break;
                        case "btnSubmit":
                            this.Submit();
                            this.CreateInfoLog(Resource.System_Action_Save);
                            break;
                        case "btnExport":
                            this.Export();
                            this.CreateInfoLog(Resource.System_Action_Export);
                            break;
                        case "btnInport":
                            this.Inport();
                            this.CreateInfoLog(Resource.System_Action_Inport);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                this.CreateErrorLog(ex);
            }

        }

        protected void CreateErrorLog(Exception throwEx)
        {
            var log = new ErrorLog
                {
                    Details = throwEx.StackTrace,
                    UserName = this.User.Identity.Name,
                    Ip = Net.GetClientIP(),
                    PageCode = 0,
                    Message = throwEx.Message,
                    Page = this.module
                };
            log.Create();
        }

        protected void CreateInfoLog(string action)
        {
            var log = new InfoLog { UserName = this.User.Identity.Name, Action = action, Page = this.module };
            log.Create();
        }

        protected virtual void Update() { }

        protected virtual void Delete() { }

        protected virtual void Add() { }

        protected virtual void Save() { }

        protected virtual void Submit() { }
        protected virtual void Reset() { }

        protected virtual void Query() { }

        protected virtual void Export() { }

        protected virtual void Inport() { }

        #endregion 按钮事件
    }
}