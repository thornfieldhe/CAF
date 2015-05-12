using Shareidea.Web.Component.Workflow;
using Shareidea.Web.UI.Control.Workflow.Designer;
using Shareidea.Web.UI.Control.Workflow.Designer.Component;
using Shareidea.Web.UI.Control.Workflow.Designer.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
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
namespace Shareidea.Web.UI.Control.Workflow.Setting
{
    public partial class ActivitySetting : UserControl
    {
        public void ApplyCulture()
        {
            tbActivityName.Text = Text.ActivityName;
            tbActivityType.Text = Text.ActivityType;
            btnAppay.Content = Text.Button_Apply;
            btnClose.Content = Text.Button_Cancle;
            btnSave.Content = Text.Button_OK;
            tbMergePictureRepeatDirection.Text = Text.RepeatDirection;
            initActivityList();
            initMergePictureRepeatDirection();

            if (currentActivity != null)
            {

                initSetting(currentActivity.ActivityData);
            }
        }
        Activity currentActivity;
        public void SetSetting(Activity a)
        {
            this.Visibility = Visibility.Visible;
            this.ShowDisplayAutomation();
            if (a == currentActivity)
                return;
            clearSetting();
            initSetting(a.ActivityData);
            currentActivity = a;
            this.cbPost.ItemsSource = currentActivity.Container.PostList;
        }
        void clearSetting()
        {
            txtActivityName.Text = "";
            cbActivityType.SelectedIndex = -1;
            cbPost.SelectedIndex = -1;
        }
        void initSetting(ActivityComponent ac)
        {
            txtActivityName.Text = ac.ActivityName;
            string name = "";
            for (int i = 0; i < cbActivityType.Items.Count; i++)
            {
                name = ((ActivityTypeItem)cbActivityType.Items[i]).Name;

                if (name == ac.ActivityType)
                {
                    cbActivityType.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < cbMergePictureRepeatDirection.Items.Count; i++)
            {
                name = ((RepeatDirectionItem)cbMergePictureRepeatDirection.Items[i]).Name;

                if (name == ac.RepeatDirection)
                {
                    cbMergePictureRepeatDirection.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < cbPost.Items.Count; i++)
            {
                name = ((WorkflowListItem)cbPost.Items[i]).ID;
                if (name == ac.ActivityPost)
                {
                    cbPost.SelectedIndex = i;
                    break;
                }
            }

            ActivityType t = (ActivityType)Enum.Parse(typeof(ActivityType), ac.ActivityType, true);
            if (t == ActivityType.OR_MERGE
                || t == ActivityType.AND_MERGE
                || t == ActivityType.VOTE_MERGE)
            {
                tbMergePictureRepeatDirection.Visibility = Visibility.Visible;
                cbMergePictureRepeatDirection.Visibility = Visibility.Visible;
            }
            else
            {
                tbMergePictureRepeatDirection.Visibility = Visibility.Collapsed;
                cbMergePictureRepeatDirection.Visibility = Visibility.Collapsed;
            }

        }
        void initActivityList()
        {
            List<ActivityTypeItem> Cus = new List<ActivityTypeItem>();
            Cus.Add(new ActivityTypeItem("INTERACTION", Text.ActivityType_INTERACTION));
            Cus.Add(new ActivityTypeItem("AND_BRANCH", Text.ActivityType_AND_BRANCH));
            Cus.Add(new ActivityTypeItem("OR_BRANCH", Text.ActivityType_OR_BRANCH));
            Cus.Add(new ActivityTypeItem("AND_MERGE", Text.ActivityType_AND_MERGE));
            Cus.Add(new ActivityTypeItem("OR_MERGE", Text.ActivityType_OR_MERGE));
            Cus.Add(new ActivityTypeItem("VOTE_MERGE", Text.ActivityType_VOTE_MERGE));
            Cus.Add(new ActivityTypeItem("AUTOMATION", Text.ActivityType_AUTOMATION));
            Cus.Add(new ActivityTypeItem("INITIAL", Text.ActivityType_INITIAL));
            Cus.Add(new ActivityTypeItem("COMPLETION", Text.ActivityType_COMPLETION));
            cbActivityType.ItemsSource = Cus;
        }
        public ActivitySetting()
        {
            InitializeComponent();
            initActivityList();
            initMergePictureRepeatDirection();
            initPostList();
        }
        public class RepeatDirectionItem
        {
            public string Name { get; set; }
            public string Text { get; set; }
            public RepeatDirectionItem()
            {
            }
            public RepeatDirectionItem(string name, string text)
            {
                Name = name;
                Text = text;
            }
        }
        void initMergePictureRepeatDirection()
        {
            List<RepeatDirectionItem> Cus = new List<RepeatDirectionItem>();

            Cus.Add(new RepeatDirectionItem("Horizontal", Text.RepeatDirection_Horizontal));
            Cus.Add(new RepeatDirectionItem("Vertical", Text.RepeatDirection_Vertical));
            cbMergePictureRepeatDirection.ItemsSource = Cus;
            cbMergePictureRepeatDirection.SelectedIndex = 0;
        }

        public class ActivityTypeItem
        {
            public string Name { get; set; }
            public string Text { get; set; }
            public ActivityTypeItem(string name, string text)
            {
                Name = name;
                Text = text;
            }
        }
        ActivityComponent getActivityData()
        {
            ActivityComponent ac = new ActivityComponent();
            ac.ActivityName = txtActivityName.Text;
            if (cbActivityType.SelectedIndex >= 0)
            {
                ActivityTypeItem cbi = cbActivityType.SelectedItem as ActivityTypeItem;
                if (cbi != null)
                {
                    ac.ActivityType = cbi.Name;
                }

            }
            if (cbMergePictureRepeatDirection.SelectedIndex >= 0)
            {
                RepeatDirectionItem cbi = cbMergePictureRepeatDirection.SelectedItem as RepeatDirectionItem;
                if (cbi != null)
                {
                    ac.RepeatDirection = cbi.Name;
                }

            }
            if (cbPost.SelectedIndex >= 0)
            {
                WorkflowListItem cbi = cbPost.SelectedItem as WorkflowListItem;
                if (cbi != null)
                {
                    ac.ActivityPost = cbi.ID;
                }

            }
            return ac;
        }
        public void ShowDisplayAutomation()
        {
            sbActivitySettingDisplay.Begin();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

            close();
        }
        void close()
        {
            sbActivitySettingClose.Completed += new EventHandler(sbActivitySettingClose_Completed);
            sbActivitySettingClose.Begin();
        }
        void sbActivitySettingClose_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            currentActivity.SetActivityData(getActivityData());
            close();


        }
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            currentActivity.SetActivityData(getActivityData());

        }
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            FrameworkElement element = sender as FrameworkElement;
            mousePosition = e.GetPosition(null);
            trackingMouseMove = true;
            if (null != element)
            {
                element.CaptureMouse();
                element.Cursor = Cursors.Hand;
            }

        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {



            FrameworkElement element = sender as FrameworkElement;
            trackingMouseMove = false;
            element.ReleaseMouseCapture();

            mousePosition.X = mousePosition.Y = 0;
            element.Cursor = null;


        }
        bool trackingMouseMove = false;
        Point mousePosition;


        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            element.Cursor = Cursors.Hand;
            if (trackingMouseMove)
            {
                double deltaV = e.GetPosition(null).Y - mousePosition.Y;
                double deltaH = e.GetPosition(null).X - mousePosition.X;
                double newTop = deltaV + (double)this.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)this.GetValue(Canvas.LeftProperty);

                double containerWidth = (double)this.Parent.GetValue(Canvas.WidthProperty);
                double containerHeight = (double)this.Parent.GetValue(Canvas.HeightProperty);
                if (newLeft + this.Width > containerWidth
                   || newTop + this.Height > containerHeight
                    || newLeft < 0
                    || newTop < 0
                    )
                {
                    //超过流程容器的范围
                }
                else
                {



                    this.SetValue(Canvas.TopProperty, newTop);
                    this.SetValue(Canvas.LeftProperty, newLeft);

                    mousePosition = e.GetPosition(null);
                }
            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbActivityType.SelectedItem != null)
            {
                ActivityType t = (ActivityType)Enum.Parse(typeof(ActivityType), ((ActivityTypeItem)cbActivityType.SelectedItem).Name, true);
                if (t == ActivityType.OR_MERGE
                    || t == ActivityType.AND_MERGE
                    || t == ActivityType.VOTE_MERGE)
                {
                    tbMergePictureRepeatDirection.Visibility = Visibility.Visible;
                    cbMergePictureRepeatDirection.Visibility = Visibility.Visible;
                }
                else
                {
                    tbMergePictureRepeatDirection.Visibility = Visibility.Collapsed;
                    cbMergePictureRepeatDirection.Visibility = Visibility.Collapsed;
                }
            }
        }
        private bool postIsCreated;

        void initPostList()
        {
            if (postIsCreated)
                return;
            postIsCreated = true;
        }

        void _workflowClient_GetPostListCompleted(object sender, Shareidea.Web.UI.Control.Workflow.Designer.ServiceReference.GetPostListCompletedEventArgs e)
        {
            if (e.Result == "")
                return;

            Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(e.Result);
            XElement xele = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));
            var partNos = from item in xele.Descendants("Post")
                          select new WorkflowListItem
                          {
                              Name = item.Attribute("postname").Value,
                              ID = item.Attribute("ID").Value
                          };
            cbPost.ItemsSource = partNos;
        }
    }
}
