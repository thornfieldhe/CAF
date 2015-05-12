using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Shareidea.Web.UI.Control.Workflow.Designer.Resources;
//*******************************************************************
//                                                                  *
//              http://www.shareidea.net                            *
//                                                                  *
//         Copyright @ 深圳市吉软科技有限公司 2009                  *
//                                                                  *
// 本程序使用GPLv2协议发布，如果您使用本软件，表示您遵守此协议      *
//                                                                  *
//                  请保留本段声明                                  *
//                                                                  *
//*******************************************************************
namespace Shareidea.Web.UI.Control.Workflow.Designer
{
    public partial class RuleMenu : UserControl
    {
        IContainer _container;
        public IContainer Container
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;
            }
        }

        public Point CenterPoint
        {
            get
            {
                return new Point((double)this.GetValue(Canvas.LeftProperty), (double)this.GetValue(Canvas.TopProperty));
            }
            set
            {
                this.SetValue(Canvas.TopProperty, value.Y);
                this.SetValue(Canvas.LeftProperty, value.X);
            }
        }
        public void ApplyCulture()
        {
            btnCopy.Content = Text.Menu_CopyRule;
            btnDelete.Content = Text.Menu_DeleteRule;
            btnShowRuleSetting.Content = Text.Menu_ModifyRule;

        }
        public RuleMenu()
        {
            InitializeComponent();

        }
        Rule relatedRule;
        public Rule RelatedRule
        {
            get
            {
                return relatedRule;
            }
            set
            {
                relatedRule = value;
            }
        }
        private void deleteRule(object sender, RoutedEventArgs e)
        {
            if (relatedRule != null)
            {
                if (System.Windows.Browser.HtmlPage.Window.Confirm(Text.Comfirm_Delete))
                {
                    this.Visibility = Visibility.Collapsed;
                    if (isMultiControlSelect && _container.CurrentSelectedControlCollection != null
                        && _container.CurrentSelectedControlCollection.Count > 0)
                    {
                        IElement iel;
                        foreach (System.Windows.Controls.Control c in _container.CurrentSelectedControlCollection)
                        {
                            iel = c as IElement;
                            if (iel != null)
                            {
                                iel.Delete();
                            }
                        }
                    }
                    relatedRule.Delete();
                    _container.SaveChange(HistoryType.New);


                }
            }
        }
        System.Windows.Threading.DispatcherTimer _menuTimer;
        void Menu_Timer(object sender, EventArgs e)
        {
            if (_menuTimer != null && _menuTimer.IsEnabled)
                _menuTimer.Stop();
            ShowMenu(Visibility.Collapsed);

        }
        bool isMultiControlSelect = false;
        public void ShowMenu(Visibility visible)
        {

            isMultiControlSelect = false;
            if (visible == Visibility.Visible)
            {
                if (_menuTimer == null)
                {
                    _menuTimer = new System.Windows.Threading.DispatcherTimer();
                    _menuTimer.Interval = new TimeSpan(0, 0, 0, 2, 0);
                    _menuTimer.Tick += new EventHandler(Menu_Timer);
                }
                _menuTimer.Start();
                if (_container.CurrentSelectedControlCollection != null
                    && _container.CurrentSelectedControlCollection.Count > 0
                    )
                {
                    if (!_container.CurrentSelectedControlCollection.Contains(relatedRule))
                    {
                        _container.ClearSelectFlowElement(null);
                        btnShowRuleSetting.IsEnabled = true;
                        isMultiControlSelect = false;

                    }
                    else
                    {
                        btnShowRuleSetting.IsEnabled = false;
                        isMultiControlSelect = true;

                    }
                }
                else
                {
                    btnShowRuleSetting.IsEnabled = true;
                    isMultiControlSelect = false;
                }
                if (isMultiControlSelect)
                {
                    btnDelete.Content = Text.Menu_DeleteSelected;
                    btnCopy.Content = Text.Menu_CopySelected;

                }
                else
                {
                    btnDelete.Content = Text.Menu_DeleteRule;
                    btnCopy.Content = Text.Menu_CopyRule;

                }
                this.Visibility = visible;

                sbShowMenu.Begin();


            }
            else
            {
                sbCloseMenu.Completed += new EventHandler(sbCloseMenu_Completed);
                sbCloseMenu.Begin();
            }
        }
        void sbCloseMenu_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        private void showRuleSetting(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            _container.ShowRuleSetting(relatedRule);

        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            ShowMenu(Visibility.Collapsed);

        }
        private void copyActivity(object sender, RoutedEventArgs e)
        {
            if (isMultiControlSelect)
            {
                _container.CopySelectedControlToMemory(null);
            }
            else
            {
                _container.CopySelectedControlToMemory(relatedRule);

            }
            this.Visibility = Visibility.Collapsed;

        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_menuTimer != null && _menuTimer.IsEnabled)
            {
                _menuTimer.Stop();
                _menuTimer = null;
            }
        }
    }
}
