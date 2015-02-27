using FineUI;
using System;
using System.Collections.Generic;
using System.Web;
namespace CAF.Web.WebForm.Common
{
    using CAF.Model;
    using System.Web.UI;

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
                var id = Guid.NewGuid();
                return string.IsNullOrWhiteSpace(Request["Id"]) ? id : Guid.Parse(Request["Id"]);
            }
        }

        protected List<Guid> readRule;
        protected string writeRule = "";

        public BasePage(Guid pageId)
        {
            this.pageId = pageId;
        }

//        protected Model.User LoginUser
//        {
//            get
//            {
//                return (Model.User)HttpContext.Current.Session["User"];
//            }
//        }

        #region 控件初始化

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            //            this.module = Directory.Get(pageId).Name;
            if (IsPostBack)
            {
                return;
            }
            try
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var canRead = CanRead();
                    var canEdit = CanEdit();

                    if (canRead || canEdit)
                    {
                        this.Title = module.ToString();
                        Initialization();
                        if (canEdit)
                        {
                            InitializationEditControls();
                        }
                        else
                        {
                            InitializationReadControls();
                        }

                        CreateInfoLog(Resource.System_Action_LoginIn);
                    }
                    else
                    {
                        UnAuthenticated();
                    }
                }
                else
                {
                    UnAuthenticated();
                }
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);
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
            Bind();
            BindScripts();
        }

 

        /// <summary>
        /// 具体控件绑定工作
        /// </summary>
        protected virtual void Bind() { }

        /// <summary>
        /// 绑定脚本
        /// </summary>
        protected virtual void BindScripts() { }

        /// <summary>
        /// 绑定只读模式下控件的状态
        /// </summary>
        protected virtual void InitializationReadControls()
        {
            InitializationReadControls(this.Page);
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
                    InitializationReadControls(item);
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
                    button.ID.Contains("Reset"))
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

        protected virtual bool CanRead()
        {
            //            List<Guid> items = Sys_Dictionary.GetKeyByCategoryAndValueAndStatus(EnumExt.GetDescription(DictionaryEnum.RoleToDir), pageId, (int)RightStatusEnum.Read);
            //            return RuleValidate(items);
            return true;
        }

        protected virtual bool CanEdit()
        {
            //            List<Guid> items = Sys_Dictionary.GetKeyByCategoryAndValueAndStatus(EnumExt.GetDescription(DictionaryEnum.RoleToDir), pageId, (int)RightStatusEnum.Write);
            //            return RuleValidate(items);
            return true;
        }

        private bool RuleValidate(List<Guid> rules)
        {
            var result = false;
            if (rules == null || rules.Count == 0)
            {
                return false;
            }
            //            CAFPrincipal principal = (CAFPrincipal)HttpContext.Current.Session["Principal"];
            foreach (Guid item in rules)
            {
                //                if (principal.IsInRole(item.ToString()))
                //                {
                //                    return true;
                //                }
            }
            return result;
        }

        protected virtual void UnAuthenticated()
        {
            Response.Redirect("~/Login.aspx?referrer=" + Server.UrlEncode(Request.Url.PathAndQuery));
            //Response.End();
            //return;
        }

        #endregion 授权

        #region 按钮事件

        protected virtual void btnExcute_Click(object sender, EventArgs e)
        {
            var throwEx = new Exception("");
            try
            {
                if (sender is Button)
                {
                    var btn = (Button)sender;
                    switch (btn.ID)
                    {
                        case "btnAdd":
                            PreAdd();
                            Add();
                            PostAdd();
                            CreateInfoLog(Resource.System_Action_Add);
                            Alert.ShowInTop(Resource.System_Message_AddSuccess);
                            break;
                        case "btnUpdate":
                            PreUpdate();
                            Update();
                            PostUpdate();
                            CreateInfoLog(Resource.System_Action_Update);
                            Alert.ShowInTop(Resource.System_Message_UpdateSuccess);
                            break;
                        case "btnDelete":
                            PreDelete();
                            Delete();
                            PostDelete();
                            CreateInfoLog(Resource.System_Action_Delete);
                            Alert.ShowInTop(Resource.System_Message_DeleteSuccess);
                            break;

                        case "btnReset":
                            Reset();
                            break;
                        case "btnQuery":
                            Query();
                            CreateInfoLog(Resource.System_Action_Query);
                            break;
                        case "btnSave":
                            PreSave();
                            Save();
                            PostSave();
                            CreateInfoLog(Resource.System_Action_Save);
                            Alert.ShowInTop(Resource.System_Message_SavedSuccess);
                            break;
                        case "btnSubmit":
                            PreSubmit();
                            Submit();
                            PostSubmit();
                            CreateInfoLog(Resource.System_Action_Save);
                            Alert.ShowInTop(Resource.System_Message_ConfirmSubmit);
                            break;
                        case "btnExport":
                            Export();
                            CreateInfoLog(Resource.System_Action_Export);
                            Alert.ShowInTop(Resource.System_Message_ExportSuccess);
                            break;
                        case "btnInport":
                            Inport();
                            CreateInfoLog(Resource.System_Action_Inport);
                            Alert.ShowInTop(Resource.System_Message_InportSuccess);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                throwEx = ex;
            }
            if (throwEx.Message != "")
            {
                CreateErrorLog(throwEx);
            }
        }

        protected void CreateErrorLog(Exception throwEx)
        {
            //            Sys_ErrorLog log = Sys_ErrorLog.New();
            //            log.Details = throwEx.StackTrace;
            //            log.UserName = User.Identity.Name;
            //            log.Page = pageId;
            //            log.Ip = CAF.Web.Net.GetClientIP();
            //            log.PageCode = 0;
            //            log.Message = throwEx.Message;
            //            log.Create();
        }

        protected void CreateInfoLog(string action)
        {
            //            Sys_InfoLog log = Sys_InfoLog.New();
            //            log.UserName = User.Identity.Name;
            //            log.Page = pageId;
            //            log.Action = action.ToString();
            //            log.Create();
        }

        protected virtual void Update() { }

        protected virtual void Delete() { }

        protected virtual void Add() { }

        protected virtual void Save() { }

        protected virtual void Submit() { }

        protected virtual void PreDelete() { }

        protected virtual void PreUpdate() { }

        protected virtual void PreAdd() { }
        protected virtual void PreSubmit() { }

        protected virtual void PreSave() { }

        protected virtual void PostDelete() { }

        protected virtual void PostUpdate() { }

        protected virtual void PostSubmit() { }

        protected virtual void PostAdd() { }

        protected virtual void PostSave() { }

        protected virtual void Reset() { }

        protected virtual void Query() { }

        protected virtual void Export() { }

        protected virtual void Inport() { }

        #endregion 按钮事件
    }
}