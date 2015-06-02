using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Xml;
using CAF.Model;
using FineUI;

namespace CAF.Web
{
    using CAF;
    using CAF.Web;
    using CAF.Webs;

    using Newtonsoft.Json.Linq;

    public partial class Dashboard : Page
    {
        private List<string> dirLevels;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                this.Response.Redirect("Login.aspx");
            }
            var u = CAF.Model.User.Get(this.User.Identity.Name);
            if (u != null)
            {
                this.lblUserName.Text = u.Name;
                this.GetDirLevels(CAF.Model.User.GetDirectories(u.Id));
                var ids = this.GetClientIDS(this.mainTabStrip);
                var accordionMenu = this.InitAccordionMenu();
                ids.Add("mainMenu", accordionMenu.ClientID);
                ids.Add("menuType", "accordion");
                // 只在页面第一次加载时注册客户端用到的脚本
                if (this.Page.IsPostBack)
                {
                    return;
                }
                var idsScriptStr = String.Format("window.IDS={0};", ids.ToString(global::Newtonsoft.Json.Formatting.None));
                PageContext.RegisterStartupScript(idsScriptStr);

                this.btnChangePass.OnClientClick = this.winChangePass.GetShowReference("~/System/User_ChangePass.aspx?Id=" + u.Id, string.Format("基本信息维护一{0}", u.Name));
            }
            else
            {
                this.Response.Redirect("~/Login.aspx?referrer=" + this.Server.UrlEncode(this.Request.Url.PathAndQuery));
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            var log = new LoginLog { UserName = this.User.Identity.Name, Ip = Web.GetClientIP(), Status = (int)LoginStatusEnum.LoginOut };

            log.Create();
            CAFPrincipal.Logout();
            System.Web.Security.FormsAuthentication.SignOut();

            this.Response.Redirect("Login.aspx");
        }

        private JObject GetClientIDS(params ControlBase[] ctrls)
        {
            var jo = new JObject();
            foreach (var ctrl in ctrls)
            {
                jo.Add(ctrl.ID, ctrl.ClientID);
            }

            return jo;
        }

        private Accordion InitAccordionMenu()
        {
            var accordionMenu = new Accordion
                                    {
                                        ID = "accordionMenu",
                                        EnableFill = true,
                                        ShowBorder = false,
                                        ShowHeader = false
                                    };
            this.Region2.Items.Add(accordionMenu);

            var dirs = Directory.GetAllByParentId(new Guid()).OrderBy(d => d.Sort);

            foreach (var dir in dirs)
            {

                if (Directory.GetAllByParentId(dir.Id).Count > 0 && this.dirLevels.Contains(dir.Level))
                {
                    var accordionPane = new AccordionPane
                                            {
                                                Title = dir.Name,
                                                Layout = Layout.Fit,
                                                ShowBorder = false,
                                                BodyPadding = "2px 0 0 0"
                                            };
                    accordionMenu.Items.Add(accordionPane);

                    var innerTree = new Tree
                                        {
                                            EnableArrows = true,
                                            ShowBorder = false,
                                            ShowHeader = false,
                                            EnableIcons = true,
                                            AutoScroll = true,
                                            EnableSingleClickExpand = true
                                        };

                    accordionPane.Items.Add(innerTree);

                    // 绑定AccordionPane内部的树控件
                    innerTree.DataSource = this.CreateXmlTree(dir.Id);
                    innerTree.DataBind();
                }
            }

            return accordionMenu;
        }

        private XmlDocument CreateXmlTree(Guid parentId)
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement("Tree");
            doc.AppendChild(root);
            this.BindChildDirs(parentId, doc, root);
            return doc;
        }

        private void BindChildDirs(Guid parentId, XmlDocument doc, XmlElement element)
        {
            var dirs = Directory.GetAllByParentId(parentId).OrderBy(p => p.Sort);
            foreach (var item in dirs)
            {
                if (item.Status != (int)HideStatusEnum.Show || !this.dirLevels.Contains(item.Level))
                {
                    continue;
                }
                var e = doc.CreateElement("TreeNode");
                e.SetAttribute("Text", item.Name);
                if (string.IsNullOrWhiteSpace(item.Url))
                {
                    e.SetAttribute("SingleClickExpand", "true");
                }
                else
                {
                    e.SetAttribute("NavigateUrl", item.Url);
                }
                element.AppendChild(e);
                this.BindChildDirs(item.Id, doc, e);
            }
        }

        /// <summary>
        /// 获取用户所有目录列表
        /// </summary>
        /// <returns></returns>
        private void GetDirLevels(List<KeyValueItem<Guid, string>> dirs)
        {
            this.dirLevels = new List<string>();
            foreach (var item in dirs)
            {
                this.dirLevels.AddRange(this.GetEachLevel(item.Value));
            }
            this.dirLevels = this.dirLevels.Distinct().ToList();
        }

        /// <summary>
        /// 获取该目录及其父目录层级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private List<string> GetEachLevel(string level)
        {
            var result = new List<string>();
            for (var i = 1; i <= level.Length / 2; i++)
            {
                var item = level.Substring(0, i * 2);
                if (!result.Contains(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}