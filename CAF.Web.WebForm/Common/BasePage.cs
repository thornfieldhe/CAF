using FineUI;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;



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
                return Request["Id"].ToGuid();
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
                return (User)HttpContext.Current.Session["User"];
            }
        }

        #region 控件初始化

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            //this.module = Directory.Get(pageId).Name;
            if (IsPostBack)
            {
                return;
            }
            try
            {
                //                if (HttpContext.Current.User.Identity.IsAuthenticated)
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
                //                else
                //                {
                //                    UnAuthenticated();
                //                }
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
        }



        /// <summary>
        /// 具体控件绑定工作
        /// </summary>
        protected virtual void Bind() { }



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
            //                        List<Guid> items = Dictionary.GetKeyByCategoryAndValueAndStatus(EnumExt.GetDescription(DictionaryEnum.RoleToDir), pageId, (int)RightStatusEnum.Read);
            //                        return RuleValidate(items);
            return true;
        }

        protected virtual bool CanEdit()
        {
            //                        List<Guid> items = Dictionary.GetKeyByCategoryAndValueAndStatus(EnumExt.GetDescription(DictionaryEnum.RoleToDir), pageId, (int)RightStatusEnum.Write);
            //                        return RuleValidate(items);
            return true;
        }

        private bool RuleValidate(List<Guid> rules)
        {
            const bool result = false;
            if (rules == null || rules.Count == 0)
            {
                return false;
            }
            foreach (var item in rules)
            {
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

//        protected virtual void btnExcute_Click(object sender, EventArgs e)
//        {
//            var throwEx = new Exception("");
//            try
//            {
//                if (sender is Button)
//                {
//                    var btn = (Button)sender;
//                    string faildMessage;
//                    switch (btn.ID)
//                    {
//                        case "btnAdd":
//                            PreAdd();
//                            faildMessage = Add();
//
//                            if (string.IsNullOrWhiteSpace(faildMessage))
//                            {
//                                PostAdd();
//                                CreateInfoLog(Resource.System_Action_Add);
//                                Alert.ShowInTop(Resource.System_Message_AddSuccess);
//                            }
//                            else
//                            {
//                                Alert.ShowInTop(faildMessage);
//                            }
//
//                            break;
//                        case "btnUpdate":
//                            PreUpdate();
//                            faildMessage = Update();
//
//                            if (string.IsNullOrWhiteSpace(faildMessage))
//                            {
//                                PostUpdate();
//                                CreateInfoLog(Resource.System_Action_Update);
//                                Alert.ShowInTop(Resource.System_Message_UpdateSuccess);
//                            }
//                            else
//                            {
//                                Alert.ShowInTop(faildMessage);
//
//                            }
//                            break;
//
//                        case "btnDelete":
//                            PreDelete();
//                            faildMessage = Delete();
//
//                            if (string.IsNullOrWhiteSpace(faildMessage))
//                            {
//                                PostDelete();
//                                CreateInfoLog(Resource.System_Action_Delete);
//                                Alert.ShowInTop(Resource.System_Message_DeleteSuccess);
//                            }
//                            else
//                            {
//                                Alert.ShowInTop(faildMessage);
//
//                            }
//                            break;
//
//                        case "btnReset":
//                            Reset();
//                            break;
//                        case "btnQuery":
//                            Query();
//                            CreateInfoLog(Resource.System_Action_Query);
//                            break;
//                        case "btnSave":
//                            PreSave();
//                            faildMessage = Save();
//
//                            if (string.IsNullOrWhiteSpace(faildMessage))
//                            {
//                                PostSave();
//                                CreateInfoLog(Resource.System_Action_Save);
//                                Alert.ShowInTop(Resource.System_Message_SavedSuccess);
//                            }
//                            else
//                            {
//                                Alert.ShowInTop(faildMessage);
//
//                            }
//                            break;
//                        case "btnSubmit":
//                            PreSubmit();
//                            faildMessage = Submit();
//                            if (string.IsNullOrWhiteSpace(faildMessage))
//                            {
//                                PostSubmit();
//                                CreateInfoLog(Resource.System_Action_Save);
//                                Alert.ShowInTop(Resource.System_Message_ConfirmSubmit);
//                            }
//                            else
//                            {
//                                Alert.ShowInTop(faildMessage);
//                            }
//
//                            break;
//                        case "btnExport":
//                            faildMessage = Export();
//                            if (string.IsNullOrWhiteSpace(faildMessage))
//                            {
//                                CreateInfoLog(Resource.System_Action_Export);
//                                Alert.ShowInTop(Resource.System_Message_ExportSuccess);
//                            }
//                            else
//                            {
//                                Alert.ShowInTop(faildMessage);
//
//                            }
//
//                            break;
//                        case "btnInport":
//                            faildMessage = Inport();
//                            if (string.IsNullOrWhiteSpace(faildMessage))
//                            {
//                                CreateInfoLog(Resource.System_Action_Inport);
//                                Alert.ShowInTop(Resource.System_Message_InportSuccess);
//                            }
//                            else
//                            {
//                                Alert.ShowInTop(faildMessage);
//
//                            }
//
//                            break;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Alert.ShowInTop(ex.Message);
//                throwEx = ex;
//            }
//            if (throwEx.Message != "")
//            {
//                CreateErrorLog(throwEx);
//            }
//        }

        protected void CreateErrorLog(Exception throwEx)
        {
            var log = new ErrorLog
                {
                    Details = throwEx.StackTrace,
                    UserName = User.Identity.Name,
                    Ip = Net.GetClientIP(),
                    PageCode = 0,
                    Message = throwEx.Message,
                    Page = module
                };
            log.Create();
        }

        protected void CreateInfoLog(string action)
        {

            var log = new InfoLog { UserName = User.Identity.Name, Action = action, Page = module };
            log.Create();
        }

//        protected virtual string Update() { return string.Empty; }
//
//        protected virtual string Delete() { return string.Empty; }
//
//        protected virtual string Add() { return string.Empty; }
//
//        protected virtual string Save() { return string.Empty; }
//
//        protected virtual string Submit() { return string.Empty; }
//
//        protected virtual string PreDelete() { return string.Empty; }
//
//        protected virtual string PreUpdate() { return string.Empty; }
//
//        protected virtual string PreAdd() { return string.Empty; }
//        protected virtual string PreSubmit() { return string.Empty; }
//
//        protected virtual string PreSave() { return string.Empty; }
//
//        protected virtual string PostDelete() { return string.Empty; }
//
//        protected virtual string PostUpdate() { return string.Empty; }
//
//        protected virtual string PostSubmit() { return string.Empty; }
//
//        protected virtual string PostAdd() { return string.Empty; }
//
//        protected virtual string PostSave() { return string.Empty; }
//
//        protected virtual void Reset() { }
//
//        protected virtual void Query() { }
//
//        protected virtual string Export() { return string.Empty; }
//
//        protected virtual string Inport() { return string.Empty; }

        #endregion 按钮事件
    }
}