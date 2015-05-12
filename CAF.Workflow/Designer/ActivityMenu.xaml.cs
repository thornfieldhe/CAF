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
    public partial class ActivityMenu : UserControl
    {
        IContainer _container;
        public ActivityMenu()
        {
            InitializeComponent(); 
        }
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
        Activity relatedActivity;
        public Activity RelatedActivity
        {
            get
            {
                return relatedActivity;
            }
            set
            {
                relatedActivity = value;
            }
        }
        private void deleteActivity(object sender, RoutedEventArgs e)
        {
            if (relatedActivity != null)
            {
                if (System.Windows.Browser.HtmlPage.Window.Confirm(Text.Comfirm_Delete))
                {
                    this.Visibility = Visibility.Collapsed;
                    if (_container.CurrentSelectedControlCollection != null
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
                    relatedActivity.Delete();
                  _container.SaveChange(HistoryType.New);
                   

                }
                    

            }
        }
       public void ApplyCulture()
        {
            btnShowActivitySetting.Content = Text.Menu_ModifyActivity;
            btnCopy.Content = Text.Menu_CopyActivity;
            btnDelete.Content = Text.Menu_DeleteActivity;
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
        System.Windows.Threading.DispatcherTimer _menuTimer;
        bool isMultiControlSelect = false;
        void Menu_Timer(object sender, EventArgs e)
        {
            if (_menuTimer != null && _menuTimer.IsEnabled)
                _menuTimer.Stop();
            ShowMenu(Visibility.Collapsed);

        }
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
                    if (!_container.CurrentSelectedControlCollection.Contains(relatedActivity))
                    {
                        _container.ClearSelectFlowElement(null);
                        btnShowActivitySetting.IsEnabled = true;
                        isMultiControlSelect = false;

                    }
                    else
                    {
                        btnShowActivitySetting.IsEnabled = false;
                        isMultiControlSelect = true;

                    }
                }
                else
                {
                    btnShowActivitySetting.IsEnabled = true;
                    isMultiControlSelect = false;
                }
                if (isMultiControlSelect)
                {
                    btnDelete.Content = Text.Menu_DeleteSelected; ;
                    btnCopy.Content = Text.Menu_CopySelected; ;

                }
                else
                {
                    btnDelete.Content = Text.Menu_DeleteActivity;
                    btnCopy.Content = Text.Menu_CopyActivity;

                }
                this.Visibility = visible;
                sbShowMenu.Begin();


            }
            else
            {
                if (this.Visibility != visible)
                {
                    sbCloseMenu.Completed += new EventHandler(sbCloseMenu_Completed);
                    sbCloseMenu.Begin();
                }
            }
        }
        void sbCloseMenu_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void showActivitySetting(object sender, RoutedEventArgs e) 
        {
            this.Visibility = Visibility.Collapsed;
            _container.ShowActivitySetting(relatedActivity);

        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        { 
            ShowMenu(Visibility.Collapsed);

        }
        private void ShowSubFlow(object sender, RoutedEventArgs e)
        {
        }
        private void copyActivity(object sender, RoutedEventArgs e)
        {
            if (isMultiControlSelect)
            {
                _container.CopySelectedControlToMemory(null);
            }
            else
            {
                _container.CopySelectedControlToMemory(relatedActivity);

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