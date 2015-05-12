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
    public partial class ContainerMenu : UserControl
    {
        public ContainerMenu()
        {
            InitializeComponent();
        }
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        { 
            ShowMenu(Visibility.Collapsed);

        }
        public void ApplyCulture()
        {

            btnAddActivity.Content = Text.Button_AddActivity;
            btnAddRule.Content = Text.Button_AddRule;
            btnCopy.Content = Text.Menu_CopySelected;
            btnNext.Content = Text.Button_Next;
            btnPaste.Content = Text.Menu_Paste;
            btnPrevious.Content = Text.Button_Previous;
            btnDelete.Content = Text.Menu_DeleteSelected;

            INTERACTION.Content = Text.ActivityType_INTERACTION;
            AND_BRANCH.Content = Text.ActivityType_AND_BRANCH;
            AND_MERGE.Content = Text.ActivityType_AND_MERGE;
            AUTOMATION.Content = Text.ActivityType_AUTOMATION;
            COMPLETION.Content = Text.ActivityType_COMPLETION;
            DUMMY.Content = Text.ActivityType_DUMMY;
            INITIAL.Content = Text.ActivityType_INITIAL;
            OR_BRANCH.Content = Text.ActivityType_OR_BRANCH;
            OR_MERGE.Content = Text.ActivityType_OR_MERGE;
            SUBPROCESS.Content = Text.ActivityType_SUBPROCESS;
            VOTE_MERGE.Content = Text.ActivityType_VOTE_MERGE;

            btnAlignBottom.Content = Text.Menu_AlignBottom;
            btnAlignLeft.Content = Text.Menu_AlignLeft;
            btnAlignRight.Content = Text.Menu_AlignRight;
            btnAlignTop.Content = Text.Menu_AlignTop;
            btnAddLabel.Content = Text.Button_AddLabel;
        }
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
        void Menu_Timer(object sender, EventArgs e)
        {
            if (_menuTimer != null && _menuTimer.IsEnabled)
                _menuTimer.Stop();
            ShowMenu(Visibility.Collapsed);

        }
        System.Windows.Threading.DispatcherTimer _menuTimer;

        public void ShowMenu(Visibility visible)
        {
            if (visible == Visibility.Visible)
            {
                this.Visibility = visible;

                if (_menuTimer == null)
                {
                    _menuTimer = new System.Windows.Threading.DispatcherTimer();
                    _menuTimer.Interval = new TimeSpan(0, 0, 0, 2, 0);
                    _menuTimer.Tick += new EventHandler(Menu_Timer);
                }
                _menuTimer.Start();

                setButtonEnable();
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
            this.Visibility = Visibility.Collapsed ;
        }
        void setButtonEnable()
        {
            if (_container.CurrentSelectedControlCollection != null
                   && _container.CurrentSelectedControlCollection.Count > 0)
            {
                btnCopy.IsEnabled = true;
                btnDelete.IsEnabled = true;

                if (_container.CurrentSelectedControlCollection.Count > 1)
                {
                    btnAlignTop.IsEnabled = true;
                    btnAlignBottom.IsEnabled = true;
                    btnAlignLeft.IsEnabled = true;
                    btnAlignRight.IsEnabled = true;
                }
                else
                {
                    btnAlignTop.IsEnabled = false;
                    btnAlignBottom.IsEnabled = false;
                    btnAlignLeft.IsEnabled = false;
                    btnAlignRight.IsEnabled = false;
                }
                
            }
            else
            {
                btnCopy.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnAlignTop.IsEnabled = false;
                btnAlignBottom.IsEnabled = false;
                btnAlignLeft.IsEnabled = false;
                btnAlignRight.IsEnabled = false;
            }

            if (_container.CopyElementCollectionInMemory != null
               && _container.CopyElementCollectionInMemory.Count > 0)
            {
                btnPaste.IsEnabled = true;
            }
            else
            {
                btnPaste.IsEnabled = false;
            }
            if (_container.WorkFlowXmlPreStack.Count == 0)
            {
                btnPrevious.IsEnabled = false;
            }
            else
                btnPrevious.IsEnabled = true;

            if (_container.WorkFlowXmlNextStack.Count == 0)
            {
                btnNext.IsEnabled = false;
            }
            else
                btnNext.IsEnabled = true;
        }
        private void btnAddActivity_Click(object sender, RoutedEventArgs e)
        {
           
            addAcitivty(ActivityType.INTERACTION);

        }
        private void btnAddRule_Click(object sender, RoutedEventArgs e)
        {
            Rule r = new Rule(_container);
            r.SetValue(Canvas.ZIndexProperty, _container.NextMaxIndex);
            r.RuleName = Text.NewRule + _container.NextNewRuleIndex.ToString();

            _container.AddRule(r);
            r.SetRulePosition(new Point(CenterPoint.X - 20, CenterPoint.Y - 20), new Point(CenterPoint.X + 30, CenterPoint.Y + 30),null,null);
            _container.SaveChange(HistoryType.New);
            setButtonEnable();
            ShowMenu(Visibility.Collapsed);

        }
        private void btnAddLabel_Click(object sender, RoutedEventArgs e)
        {
            Label l = new Label(_container);
            l.LabelName = Text.NewLable + _container.NextNewLabelIndex.ToString();

            _container.AddLabel(l);
            l.Position = new Point(CenterPoint.X - 20, CenterPoint.Y - 20);
            _container.SaveChange(HistoryType.New);
            setButtonEnable();
            ShowMenu(Visibility.Collapsed);

        }
        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            _container.CopySelectedControlToMemory(null);
            setButtonEnable();
            ShowMenu(Visibility.Collapsed);

        }
        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            _container.PastMemoryToContainer();
            setButtonEnable();
            ShowMenu(Visibility.Collapsed);
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            _container.PreviousAction();
            setButtonEnable();
            ShowMenu(Visibility.Collapsed);


        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _container.NextAction();
            setButtonEnable();
            ShowMenu(Visibility.Collapsed);


        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Browser.HtmlPage.Window.Confirm(Text.Comfirm_Delete))
            {
                _container.DeleteSeletedControl();
                _container.SaveChange(HistoryType.New);
            }
            //setButtonEnable();
            ShowMenu(Visibility.Collapsed);


        }

        void addAcitivty(ActivityType type)
        {
            Activity a = new Activity(_container, type);
            a.SetValue(Canvas.ZIndexProperty, _container.NextMaxIndex);
            a.ActivityName = Text.NewActivity + _container.NextNewActivityIndex.ToString();
            _container.AddActivity(a);
            a.CenterPoint = this.CenterPoint;
            _container.SaveChange(HistoryType.New);
            setButtonEnable();

            ShowMenu(Visibility.Collapsed);
        }

        private void AddActivitySubMenu_Click(object sender, RoutedEventArgs e)
        {
            ActivityType type =(ActivityType) Enum.Parse(typeof(ActivityType), ((HyperlinkButton)sender).Name, true);
            addAcitivty(type);
        }
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_menuTimer != null && _menuTimer.IsEnabled)
            {
                _menuTimer.Stop();
                _menuTimer = null;
            }
        } 
        private void Path_MouseEnter(object sender, MouseEventArgs e)
        {
            if (bdSubMenu.Visibility == Visibility.Collapsed)
            { 
                bdSubMenu.Visibility = Visibility.Visible;
                sbShowSubMenu.Begin();
            }
        }

        

        void sbCloseSubMenu_Completed(object sender, EventArgs e)
        {
            bdSubMenu.Visibility = Visibility.Collapsed;
           
        }
        void closeSubMenu()
        {
            if (bdSubMenu.Visibility == Visibility.Visible)
            {
                sbCloseSubMenu.Completed += new EventHandler(sbCloseSubMenu_Completed);
                sbCloseSubMenu.Begin();
            }
        }
        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            closeSubMenu();
        }

        private void HyperlinkButton_MouseEnter(object sender, MouseEventArgs e)
        {
            closeSubMenu();
        }
        private void btnTopOrderliness_Click(object sender, RoutedEventArgs e)
        {
            
                _container.AlignTop();
                _container.SaveChange(HistoryType.New); 


        }
        private void btnLeftOrderliness_Click(object sender, RoutedEventArgs e)
        {
            _container.AlignLeft();
            _container.SaveChange(HistoryType.New); 


        }
        private void btnBottomOrderliness_Click(object sender, RoutedEventArgs e)
        {

            _container.AlignBottom();
            _container.SaveChange(HistoryType.New);


        }
        private void btnRightOrderliness_Click(object sender, RoutedEventArgs e)
        {
            _container.AlignRight();
            _container.SaveChange(HistoryType.New);


        }
    }
}
