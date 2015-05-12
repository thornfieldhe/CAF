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
using Shareidea.Web.Component.Workflow;
using Shareidea.Web.UI.Control.Workflow.Designer;
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
namespace Shareidea.Web.UI.Control.Workflow.Setting
{
    public partial class RuleSetting : UserControl
    {
        public void ApplyCulture()
        {

            tbCondition.Text = Text.RuleCondition;
            tbLineType.Text = Text.LineType;
            tbRuleName.Text = Text.RuleName;

            btnAppay.Content = Text.Button_Apply;
            btnClose.Content = Text.Button_Cancle;
            btnSave.Content = Text.Button_OK;
            initLineType();

            if (currentRule != null)
            {
                initSetting(currentRule.RuleData);
            }
        }
        Rule currentRule;
        public void SetSetting(Rule r)
        {
            this.Visibility = Visibility.Visible;
            this.ShowDisplayAutomation();
            if (r == currentRule)
                return;
            clearSetting();
            initSetting(r.RuleData);
            currentRule = r;
        }
        public void ShowDisplayAutomation()
        {
            sbRuleSettingDisplay.Begin();
        }
        void close()
        {
            sbRuleSettingClose.Completed += new EventHandler(sbActivitySettingClose_Completed);
            sbRuleSettingClose.Begin();
        }
        void sbActivitySettingClose_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        void clearSetting()
        {
            txtRuleName.Text = "";
            txtRuleCondition.Text = "";
        }
        void initSetting(RuleComponent rc)
        {
            txtRuleName.Text = rc.RuleName;
            txtRuleCondition.Text = rc.RuleCondition;

            string name = "";
            for (int i = 0; i < cbRuleLineType.Items.Count; i++)
            {
                name = ((RuleLineTypeItem)cbRuleLineType.Items[i]).Name;

                if (name == rc.LineType)
                {
                    cbRuleLineType.SelectedIndex = i;
                    break;
                }
            }
        }

        RuleComponent getRuleData()
        {
            RuleComponent rc = new RuleComponent();
            rc.RuleName = txtRuleName.Text;
            rc.RuleCondition = txtRuleCondition.Text;

            if (cbRuleLineType.SelectedIndex >= 0)
            {
                RuleLineTypeItem cbi = cbRuleLineType.SelectedItem as RuleLineTypeItem;
                if (cbi != null)
                {
                    rc.LineType = cbi.Name;
                }

            }
            return rc;
        }
        void initLineType()
        {
            List<RuleLineTypeItem> Cus = new List<RuleLineTypeItem>();


            Cus.Add(new RuleLineTypeItem("Line", Text.RuleLineType_Line));
            Cus.Add(new RuleLineTypeItem("Polyline", Text.RuleLineType_Polyline));


            cbRuleLineType.ItemsSource = Cus;
        }
        public RuleSetting()
        {
            InitializeComponent();
            initLineType();


        }
        public class RuleLineTypeItem
        {
            public string Name { get; set; }
            public string Text { get; set; }
            public RuleLineTypeItem(string name, string text)
            {
                Name = name;
                Text = text;
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            currentRule.SetRuleData(getRuleData());
            close();
            

        }
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            currentRule.SetRuleData(getRuleData());

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
    }
}
