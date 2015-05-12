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
    public enum RuleMoveType { Begin = 0, Line, End }
    public delegate void RuleChangeDelegate(Rule r);

    public partial class Rule : UserControl, IElement
    {
        Point origBeginPoint;
        Point origEndPoint;
        Point origTurnPoint1Point;
        Point origTurnPoint2Point;
        bool positionIsChange = true;
        public void Zoom(double zoomDeep)
        {

            if (positionIsChange)
            {
                origBeginPoint = BeginPointPosition;
                origEndPoint = EndPointPosition;
                origTurnPoint1Point = RuleTurnPoint1.CenterPosition;
                origTurnPoint2Point = RuleTurnPoint2.CenterPosition;
                positionIsChange = false;
            }
            if (BeginActivity == null)
                BeginPointPosition = new Point(origBeginPoint.X * zoomDeep, origBeginPoint.Y * zoomDeep);
            if (EndActivity == null)
                EndPointPosition = new Point(origEndPoint.X * zoomDeep, origEndPoint.Y * zoomDeep);
            if (LineType == RuleLineType.Polyline)
            {
                RuleTurnPoint1.CenterPosition = new Point(origTurnPoint1Point.X * zoomDeep, origTurnPoint1Point.Y * zoomDeep);
                onRuleTurn1PointMove(RuleTurnPoint1.CenterPosition);
                RuleTurnPoint2.CenterPosition = new Point(origTurnPoint2Point.X * zoomDeep, origTurnPoint2Point.Y * zoomDeep);
                onRuleTurn2PointMove(RuleTurnPoint2.CenterPosition);
            }
        }

        bool isPassCheck
        {
            set
            {
                if (value)
                {


                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Color.FromArgb(255, 0, 128, 0);
                    begin.Fill = brush;
                    endArrow.Stroke = brush;
                    line.Stroke = brush;
                }
                else
                {

                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Color.FromArgb(255, 255, 0, 0);
                    begin.Fill = brush;
                    endArrow.Stroke = brush;
                    line.Stroke = brush;
                }
            }
        }
        void setRuleNameControlPosition()
        {
            double top = 0;
            double left = 0;
            if (this.LineType == RuleLineType.Line)
            {
                left = (BeginPointPosition.X + EndPointPosition.X) / 2;
                top = (BeginPointPosition.Y + EndPointPosition.Y) / 2;
            }
            else
            {
                left = (line.Points[1].X + line.Points[2].X) / 2;
                top = (line.Points[1].Y + line.Points[2].Y) / 2;
            }
            tbRuleName.SetValue(Canvas.TopProperty, top - 15);
            tbRuleName.SetValue(Canvas.LeftProperty, left - 10);
        }
        public CheckResult CheckSave()
        {
            CheckResult cr = new CheckResult();
            cr.IsPass = true;

            if (BeginActivity == null && EndActivity == null)
            {
                cr.Message += Text.Message_MustBeLinkToBeginAndEndActivity;
                cr.IsPass = false;
            }
            else
            {
                if (BeginActivity == null)
                {
                    cr.Message += Text.Message_MustBeLinkToBeginActivity;
                    cr.IsPass = false;
                }
                if (EndActivity == null)
                {
                    cr.Message += Text.Message_MustBeLinkToEndActivity;
                    cr.IsPass = false;
                }
            }
            isPassCheck = cr.IsPass;
            if (!cr.IsPass)
            {
                errorTipControl.Visibility = Visibility.Visible;
                errorTipControl.ErrorMessage = cr.Message;
            }
            else
            {
                if (_errorTipControl != null)
                {
                    _errorTipControl.Visibility = Visibility.Collapsed;
                    cnRuleContainer.Children.Remove(_errorTipControl);
                    _errorTipControl = null;
                }
            }
            return cr;
        }
        public void UpperZIndex()
        {
            ZIndex = _container.NextMaxIndex;
        }
        ErrorTip _errorTipControl;
        ErrorTip errorTipControl
        {
            get
            {
                if (_errorTipControl == null)
                {
                    _errorTipControl = new ErrorTip();
                    cnRuleContainer.Children.Add(_errorTipControl);
                    _errorTipControl.ParentElement = this;
                    _errorTipControl.SetValue(Canvas.ZIndexProperty, 1);



                }
                if (LineType == RuleLineType.Line)
                {
                    _errorTipControl.SetValue(Canvas.TopProperty, (EndPointPosition.Y + BeginPointPosition.Y) / 2 - 80);
                    _errorTipControl.SetValue(Canvas.LeftProperty, (EndPointPosition.X + BeginPointPosition.X) / 2);
                }
                else
                {
                    _errorTipControl.SetValue(Canvas.TopProperty, line.Points[1].Y - 80);
                    _errorTipControl.SetValue(Canvas.LeftProperty, line.Points[1].X);
                }
                return _errorTipControl;
            }
        }
        public Rule Clone()
        {
            Rule clone = new Rule(this._container);
            clone.originRule = this;
            clone.RuleData = new RuleComponent();
            clone.RuleData.LineType = this.RuleData.LineType;
            clone.RuleData.RuleCondition = this.RuleData.RuleCondition;
            clone.RuleData.RuleName = this.RuleData.RuleName;
            clone.LineType = this.LineType;
            clone.setUIValueByRuleData(clone.RuleData);
            clone.BeginPointPosition = this.BeginPointPosition;
            clone.EndPointPosition = this.EndPointPosition;
            clone.ZIndex = this.ZIndex;
            if (LineType == RuleLineType.Polyline)
            {
                clone.RuleTurnPoint1.CenterPosition = this.RuleTurnPoint1.CenterPosition;
                clone.RuleTurnPoint2.CenterPosition = this.RuleTurnPoint2.CenterPosition;
            }
            return clone;
        }


        RuleLineType lineType = RuleLineType.Line;
        public RuleLineType LineType
        {
            get
            {
                return lineType;
            }
            set
            {
                bool isChange = false;
                if (lineType != value)
                {
                    isChange = true;
                }
                lineType = value;
                if (isChange)
                {
                    if (LineType == RuleLineType.Line)
                    {
                        SetRulePosition(BeginPointPosition, EndPointPosition);
                    }
                    else
                    {
                        setTurnPointInitPosition();
                        SetRulePosition(BeginPointPosition, EndPointPosition, RuleTurnPoint1.CenterPosition, RuleTurnPoint2.CenterPosition);
                    }
                }
            }
        }
        public void SetRuleData(RuleComponent ruleData)
        {
            bool isChanged = false;
            if (RuleData.RuleCondition != ruleData.RuleCondition
               || RuleData.RuleName != ruleData.RuleName
                || RuleData.LineType != ruleData.LineType)
            {
                isChanged = true;

            }

            RuleData = ruleData;
            setUIValueByRuleData(ruleData);
            if (isChanged)
            {
                if (RuleChanged != null)
                    RuleChanged(this);
            }
        }

        void setUIValueByRuleData(RuleComponent ruleData)
        {


            LineType = (RuleLineType)Enum.Parse(typeof(RuleLineType), ruleData.LineType, true);
            tbRuleName.Text = ruleData.RuleName;
            RuleName = ruleData.RuleName;
        }

        RuleComponent getRuleComponentFromServer(string ruleID)
        {
            RuleComponent rc = new RuleComponent();
            rc.RuleID = this.RuleID;
            rc.UniqueID = this.UniqueID;
            rc.RuleCondition = this.RuleCondition;
            rc.RuleName = this.RuleName;
            rc.LineType = Enum.GetName(typeof(RuleLineType), LineType);
            return rc;
        }
        RuleComponent ruleData;
        public RuleComponent RuleData
        {
            get
            {
                if (ruleData == null)
                {
                    if (EditType == PageEditType.Add)
                    {
                        ruleData = new RuleComponent();
                        ruleData.RuleID = this.RuleID;
                        ruleData.UniqueID = this.UniqueID;
                        ruleData.RuleCondition = this.RuleCondition;
                        ruleData.RuleName = this.RuleName;
                        ruleData.LineType = LineType.ToString();

                    }
                    else if (EditType == PageEditType.Modify)
                    {
                        ruleData = getRuleComponentFromServer(this.RuleID);

                    }
                }
                return ruleData;
            }
            set
            {
                ruleData = value;
            }
        }


        PageEditType editType = PageEditType.None;
        public PageEditType EditType
        {
            get
            {
                return editType;
            }
            set
            {
                editType = value;
            }
        }

        public int ZIndex
        {
            get
            {
                return (int)this.GetValue(Canvas.ZIndexProperty);

            }
            set
            {
                this.SetValue(Canvas.ZIndexProperty, value);
            }

        }
        public event RuleChangeDelegate RuleChanged;

        public WorkFlowElementType ElementType
        {
            get
            {
                return WorkFlowElementType.Rule;
            }
        }
        public string ToXmlString()
        {
            System.Text.StringBuilder xml = new System.Text.StringBuilder();
            xml.Append(@"       <Rule ");
            xml.Append(@" UniqueID=""" + UniqueID + @"""");
            xml.Append(@" RuleID=""" + RuleID + @"""");
            xml.Append(@" RuleName=""" + RuleName + @"""");
            xml.Append(@" LineType=""" + LineType + @"""");
            xml.Append(@" RuleCondition=""" + RuleData.RuleCondition + @"""");
            xml.Append(@" BeginActivityUniqueID=""" + (BeginActivity == null ? "" : BeginActivity.UniqueID) + @"""");
            xml.Append(@" EndActivityUniqueID=""" + (EndActivity == null ? "" : EndActivity.UniqueID) + @"""");



            xml.Append(@" BeginPointX=""" + ((double)begin.GetValue(Canvas.LeftProperty)).ToString() + @"""");
            xml.Append(@" BeginPointY=""" + ((double)begin.GetValue(Canvas.TopProperty)).ToString() + @"""");
            xml.Append(@" EndPointX=""" + ((double)end.GetValue(Canvas.LeftProperty)).ToString() + @"""");
            xml.Append(@" EndPointY=""" + ((double)end.GetValue(Canvas.TopProperty)).ToString() + @"""");

            if (LineType == RuleLineType.Line)
            {
                xml.Append(@" TurnPoint1X=""0""");
                xml.Append(@" TurnPoint1Y=""0""");
                xml.Append(@" TurnPoint2X=""0""");
                xml.Append(@" TurnPoint2Y=""0""");
            }
            else
            {
                xml.Append(@" TurnPoint1X=""" + RuleTurnPoint1.CenterPosition.X.ToString() + @"""");
                xml.Append(@" TurnPoint1Y=""" + RuleTurnPoint1.CenterPosition.Y.ToString() + @"""");
                xml.Append(@" TurnPoint2X=""" + RuleTurnPoint2.CenterPosition.X.ToString() + @"""");
                xml.Append(@" TurnPoint2Y=""" + RuleTurnPoint2.CenterPosition.Y.ToString() + @"""");
            }
            xml.Append(@" ZIndex=""" + ZIndex + @""">");
            xml.Append(Environment.NewLine);
            xml.Append("        </Rule>");
            return xml.ToString();
        }
        public void LoadFromXmlString(string xmlString)
        {
        }
        public bool CanShowMenu
        {
            get
            {
                return canShowMenu;
            }
            set
            {
                canShowMenu = value;
            }
        }
        bool canShowMenu = false;
        public delegate void DeleteDelegate(Rule r);
        public event DeleteDelegate DeleteRule;



        bool isDeleted = false;
        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }
        }

        public void Delete()
        {



            if (!isDeleted)
            {
                isDeleted = true;
                if (this.IsTemporaryRule)
                {
                    sbBeginClose_Completed(null, null);
                }
                else
                {
                    sbBeginClose.Completed += new EventHandler(sbBeginClose_Completed);
                    sbBeginClose.Begin();
                }
            }




        }

        void sbBeginClose_Completed(object sender, EventArgs e)
        {
            if (this.EndActivity != null)
                this.EndActivity.RemoveEndRule(this);
            if (this.BeginActivity != null)
                this.BeginActivity.RemoveBeginRule(this);
            if (DeleteRule != null)
                DeleteRule(this);
            _container.RemoveRule(this);

            if (RuleChanged != null)
                RuleChanged(this);
        }

        public RuleMoveType MoveType;

        Activity beginActivity;
        Activity endActivity;
        public Activity BeginActivity
        {
            get
            {
                return beginActivity;
            }
            set
            {

                beginActivity = value;
                if (beginActivity != null)
                {
                    beginActivity.AddBeginRule(this);
                    beginActivity.ActivityMove += new MoveDelegate(OnActivityMove);
                    OnActivityMove(beginActivity, null);
                }
            }
        }
        public Activity EndActivity
        {
            get { return endActivity; }
            set
            {
                endActivity = value;

                if (endActivity != null)
                {
                    endActivity.AddEndRule(this);
                    endActivity.ActivityMove += new MoveDelegate(OnActivityMove);
                    OnActivityMove(endActivity, null);
                }

            }
        }



        public Point GetPointPosition(RuleMoveType MoveType)
        {
            Point p = new Point();
            if (MoveType == RuleMoveType.Begin)
            {
                p.X = (double)begin.GetValue(Canvas.LeftProperty);
                p.Y = (double)begin.GetValue(Canvas.TopProperty);

            }
            else if (MoveType == RuleMoveType.End)
            {
                p.X = (double)end.GetValue(Canvas.LeftProperty);
                p.Y = (double)end.GetValue(Canvas.TopProperty);

            }
            return p;
        }
        public Point GetCurrentMovedPointPosition()
        {

            return GetPointPosition(MoveType);
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
        System.Windows.Threading.DispatcherTimer _doubleClickTimer;
        public Rule(IContainer container)
            : this(container, false)
        {

        }
        public Rule(IContainer container, bool isTemporary)
            : this(container, isTemporary, RuleLineType.Line)
        {

        }
        public Rule(IContainer container, bool isTemporary, RuleLineType lineType)
        {

            InitializeComponent();
            this.IsTemporaryRule = isTemporary;
            LineType = lineType;
            editType = PageEditType.Add;
            _container = container;
            this.Name = UniqueID;

            //spContentMenu.Visibility = Visibility.Collapsed;
            System.Windows.Browser.HtmlPage.Document.AttachEvent("oncontextmenu", OnContextMenu);

            beginPointRadius = begin.Width / 2;
            begin.Height = begin.Width;

            endPointRadius = endEllipse.Width / 2;
            endEllipse.Height = endEllipse.Width;

            endArrow.SetValue(Canvas.TopProperty, endPointRadius);
            endArrow.SetValue(Canvas.LeftProperty, endPointRadius);

            if (LineType == RuleLineType.Line)
            {
                SetRulePosition(new Point(0, 0), new Point(50, 50));
            }
            else
            {
                SetRulePosition(new Point(0, 0), new Point(50, 50), RuleTurnPoint1.CenterPosition, RuleTurnPoint2.CenterPosition);

            }


            _doubleClickTimer = new System.Windows.Threading.DispatcherTimer();
            _doubleClickTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            _doubleClickTimer.Tick += new EventHandler(DoubleClick_Timer);
            if (!this.IsTemporaryRule)
            {
                sbBeginDisplay.Begin();
            }

        }
        void DoubleClick_Timer(object sender, EventArgs e)
        {
            _doubleClickTimer.Stop();
        }
        public Point BeginPointPosition
        {
            get
            {
                return GetPointPosition(RuleMoveType.Begin);
            }
            set
            {
                if (value != null && !double.IsNaN(value.X) && !double.IsNaN(value.Y))
                {
                    begin.SetValue(Canvas.TopProperty, value.Y);
                    begin.SetValue(Canvas.LeftProperty, value.X);
                    if (LineType == RuleLineType.Line)
                    {
                        SetRulePosition(BeginPointPosition, EndPointPosition);
                    }
                    else
                    {
                        SetRulePosition(BeginPointPosition, EndPointPosition, RuleTurnPoint1.CenterPosition, RuleTurnPoint2.CenterPosition);

                    }
                }

            }
        }
        Rule originRule;
        public Rule OriginRule
        {
            get
            {
                return originRule;
            }
            set
            {
                originRule = value;
            }

        }


        public Point EndPointPosition
        {
            get
            {
                return GetPointPosition(RuleMoveType.End);

            }
            set
            {
                if (value != null && !double.IsNaN(value.X) && !double.IsNaN(value.Y))
                {
                    end.SetValue(Canvas.TopProperty, value.Y);
                    end.SetValue(Canvas.LeftProperty, value.X);
                    if (LineType == RuleLineType.Line)
                    {
                        SetRulePosition(BeginPointPosition, EndPointPosition);
                    }
                    else
                    {
                        SetRulePosition(BeginPointPosition, EndPointPosition, RuleTurnPoint1.CenterPosition, RuleTurnPoint2.CenterPosition);

                    }
                }
            }
        }
        double beginPointRadius = 5;
        double endPointRadius = 5;
        private void OnContextMenu(object sender, System.Windows.Browser.HtmlEventArgs e)
        {

            if (_container.MouseIsInContainer)
            {
                e.PreventDefault();

                if (canShowMenu && !IsDeleted)
                {
                    _container.ShowRuleContentMenu(this, sender, e);

                }

            }


        }
        string uniqueID;
        public string UniqueID
        {
            get
            {
                if (string.IsNullOrEmpty(uniqueID))
                {
                    uniqueID = Guid.NewGuid().ToString();
                }
                return uniqueID;
            }
            set
            {
                uniqueID = value;
            }

        }
        string ruleID;
        public string RuleID
        {
            get
            {

                ruleID = Guid.NewGuid().ToString();
                return ruleID;
            }
            set
            {
                ruleID = value;
            }

        }
        string ruleName;
        public string RuleName
        {
            get
            {
                return ruleName;
            }
            set
            {
                ruleName = value;
                tbRuleName.Text = value;
                RuleData.RuleName = value;
            }

        }
        string ruleCondition;
        public string RuleCondition
        {
            get
            {
                return ruleCondition;
            }
            set
            {
                ruleCondition = value;
                RuleData.RuleCondition = value;
            }

        }


        public void RemoveBeginActivity(Activity a)
        {
            if (BeginActivity == a)
                BeginActivity = null;
            //需要删除事件代理 
        }





        public Point GetResetPoint(Point beginPoint, Point endPoint, Activity a, RuleMoveType type)
        {
            Point p = a.GetPointOfIntersection(beginPoint, endPoint, type);

            return p;


        }

        void OnActivityMove(Activity a, MouseEventArgs e)
        {

            if (a != EndActivity && a != BeginActivity)
                return;

            double newTop = (double)a.GetValue(Canvas.TopProperty);
            double newLeft = (double)a.GetValue(Canvas.LeftProperty);
            newTop = newTop + a.Height / 2;
            newLeft = newLeft + a.Width / 2;
            Point beginPoint = new Point();
            Point endPoint = new Point();


            beginPoint = BeginPointPosition;
            endPoint = EndPointPosition;
            if (EndActivity == a)
            {

                endPoint.X = (double)(newLeft - endPointRadius);
                endPoint.Y = (double)(newTop - endPointRadius);

                if (LineType == RuleLineType.Line)
                {

                    endPoint = GetResetPoint(beginPoint, EndActivity.CenterPoint, EndActivity, RuleMoveType.End);
                }
                else
                {
                    endPoint = GetResetPoint(RuleTurnPoint2.CenterPosition, EndActivity.CenterPoint, EndActivity, RuleMoveType.End);

                }

                if (BeginActivity != null)
                {
                    if (LineType == RuleLineType.Line)
                    {
                        beginPoint = GetResetPoint(EndActivity.CenterPoint, BeginActivity.CenterPoint, BeginActivity, RuleMoveType.Begin);
                    }
                    else
                    {
                        beginPoint = GetResetPoint(RuleTurnPoint1.CenterPosition, BeginActivity.CenterPoint, BeginActivity, RuleMoveType.Begin);

                    }

                }


            }
            else if (BeginActivity == a)
            {

                beginPoint.X = (double)(newLeft - beginPointRadius);
                beginPoint.Y = (double)(newTop - beginPointRadius);

                if (LineType == RuleLineType.Line)
                {

                    beginPoint = GetResetPoint(endPoint, BeginActivity.CenterPoint, a, RuleMoveType.Begin);
                }
                else
                {
                    beginPoint = GetResetPoint(RuleTurnPoint1.CenterPosition, BeginActivity.CenterPoint, a, RuleMoveType.Begin);

                }
                if (EndActivity != null)
                {
                    if (LineType == RuleLineType.Line)
                    {
                        endPoint = GetResetPoint(BeginActivity.CenterPoint, EndActivity.CenterPoint, EndActivity, RuleMoveType.End);
                    }
                    else
                    {
                        endPoint = GetResetPoint(RuleTurnPoint2.CenterPosition, EndActivity.CenterPoint, EndActivity, RuleMoveType.End);

                    }

                }
            }

            if (LineType == RuleLineType.Line)
            {
                SetRulePosition(beginPoint, endPoint);

            }
            else
            {
                SetRulePosition(beginPoint, endPoint, RuleTurnPoint1.CenterPosition, RuleTurnPoint2.CenterPosition);

            }
        }

        public void RemoveEndActivity(Activity a)
        {
            if (EndActivity == a)
                EndActivity = null;
            //需要删除事件代理 
        }



        bool trackingLineMouseMove = false;
        Point mousePosition;
        private void Point_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pointHadActualMove = false;
            trackingPointMouseMove = false;
            this.SetValue(Canvas.ZIndexProperty, _container.NextMaxIndex);
            FrameworkElement element = sender as FrameworkElement;

            if (element.Name == "begin")
                MoveType = RuleMoveType.Begin;
            if (element.Name == "end")
                MoveType = RuleMoveType.End;
            if (element.Name == "line")
                MoveType = RuleMoveType.Line;

            mousePosition = e.GetPosition(null);
            if (null != element)
            {

                trackingPointMouseMove = true;
                element.CaptureMouse();
                element.Cursor = Cursors.Hand;

            }
            e.Handled = true;


        }
        public void ShowMessage(string message)
        {

            _container.ShowMessage(message);
        }
        void ruleChange()
        {
            if (RuleChanged != null)
                RuleChanged(this);
        }

        void setLinkToActivity(RuleMoveType movetype)
        {
            double centerX = 0;
            double centerY = 0;

            if (movetype == RuleMoveType.Begin)
            {
                centerX = BeginPointPosition.X + beginPointRadius;
                centerY = BeginPointPosition.Y + beginPointRadius;

            }
            if (movetype == RuleMoveType.End)
            {
                centerX = EndPointPosition.X + endPointRadius;
                centerY = EndPointPosition.Y + endPointRadius;

            }

            Activity act = null;
            bool isLinked = false;
            for (int i = 0; i < _container.ActivityCollections.Count; i++)
            {

                if (isLinked)
                    break;
                act = _container.ActivityCollections[i];

                if (act.PointIsInside(new Point(centerX, centerY)))
                {
                    if (movetype == RuleMoveType.Begin)
                    {
                        #region 检查
                        if (act.Type == ActivityType.COMPLETION)
                        {
                            ShowMessage(Text.Message_EndActivityCanNotHaveFollowUpActivitiy);
                            isLinked = false;
                            break;
                        }
                        else if ((act.Type == ActivityType.AND_MERGE
                            || act.Type == ActivityType.OR_MERGE
                            || act.Type == ActivityType.VOTE_MERGE)
                            && act.BeginRuleCollections != null)
                        {

                            int count = act.BeginRuleCollections.Count;
                            if (act.BeginRuleCollections.Contains(this))
                                count--;
                            if (count > 0)
                            {
                                //ShowMessage(Text.Message_MergeActivityOnlyHaveAFollowUpActivity);
                                //isLinked = false;
                                //break;
                            }
                        }
                        #endregion
                        if (this.EndActivity == act)
                        {
                            if (!IsTemporaryRule)
                                ShowMessage(Text.Message_BeginAndEndActivityCanNotBeTheSame);
                        }
                        else
                        {
                            if (this.EndActivity == null)
                            {
                                act.AddBeginRule(this);
                                if (this.IsTemporaryRule)
                                    // this.RuleName = Text.NewRule + _container.NextNewRuleIndex.ToString();
                                    this.RuleName = "";
                                isLinked = true;
                            }
                            else
                            {
                                bool isExists = false;
                                if (act.BeginRuleCollections != null)
                                {
                                    for (int j = 0; j < act.BeginRuleCollections.Count; j++)
                                    {
                                        if (act.BeginRuleCollections[j].EndActivity == this.EndActivity
                                           && act.BeginRuleCollections[j].BeginActivity != this.BeginActivity
                                            )
                                        {
                                            isExists = true;
                                            break;
                                        }
                                    }
                                }
                                if (isExists)
                                {

                                    ShowMessage(Text.Message_TheSameRuleThatAlreadyExist);

                                }
                                else
                                {
                                    act.AddBeginRule(this);
                                    if (this.IsTemporaryRule)
                                        //this.RuleName = Text.NewRule + _container.NextNewRuleIndex.ToString();
                                        this.RuleName = "";
                                    isLinked = true;
                                }

                            }
                        }
                    }
                    if (movetype == RuleMoveType.End)
                    {
                        #region 检查

                        if (this.IsTemporaryRule)
                        {
                            if (this.BeginActivity == act)
                            {
                                isLinked = false;
                                break;
                            }
                        }

                        if (act.Type == ActivityType.INITIAL)
                        {
                            //开始活动不能有前驱活动
                            ShowMessage(Text.Message_BeginActivitiesCanNotHavePreactivity);
                            isLinked = false;
                            break;
                        }
                        else if ((act.Type == ActivityType.AND_BRANCH
                           || act.Type == ActivityType.OR_BRANCH)
                           )
                        {
                            if (act.EndRuleCollections != null
                            && act.EndRuleCollections.Count > 0)
                            {
                                int count = act.EndRuleCollections.Count;
                                if (act.EndRuleCollections.Contains(this))
                                    count--;
                                if (count > 0)
                                {
                                    ////分支活动有且只能有一个前驱活动
                                    //ShowMessage(Text.Message_BranchActivityOnlyHaveOnePreactivity);
                                    //isLinked = false;
                                    //break;
                                }
                            }
                        }
                        if (this.IsTemporaryRule)
                        {
                            if (this.BeginActivity != null)
                            {
                                if (this.BeginActivity.Type == ActivityType.COMPLETION)
                                {
                                    ShowMessage(Text.Message_EndActivityCanNotHaveFollowUpActivitiy);
                                    isLinked = false;
                                    break;
                                }
                                if ((this.BeginActivity.Type == ActivityType.AND_MERGE
                                    || this.BeginActivity.Type == ActivityType.OR_MERGE
                                    || this.BeginActivity.Type == ActivityType.VOTE_MERGE)
                                && this.BeginActivity.BeginRuleCollections != null)
                                {
                                    int count = BeginActivity.BeginRuleCollections.Count;
                                    if (this.BeginActivity.BeginRuleCollections.Contains(this))
                                        count--;
                                    if (count > 0)
                                    {
                                        ////汇聚活动只能有一个后继活动
                                        //ShowMessage(Text.Message_MergeActivityOnlyHaveAFollowUpActivity);
                                        //isLinked = false;
                                        //break;
                                    }
                                }
                            }

                        }


                        #endregion
                        if (this.BeginActivity == act)
                        {
                            if (!IsTemporaryRule)
                                ShowMessage(Text.Message_BeginAndEndActivityCanNotBeTheSame);
                        }
                        else
                        {
                            if (this.BeginActivity == null)
                            {
                                act.AddEndRule(this);
                                if (this.IsTemporaryRule)
                                    //this.RuleName = Text.NewRule + _container.NextNewRuleIndex.ToString();
                                    this.RuleName = "";
                                isLinked = true;
                            }
                            else
                            {
                                bool isExists = false;
                                if (act.EndRuleCollections != null)
                                {
                                    for (int j = 0; j < act.EndRuleCollections.Count; j++)
                                    {
                                        if (act.EndRuleCollections[j].BeginActivity == this.BeginActivity

                                           && act.EndRuleCollections[j].EndActivity != this.EndActivity
                                            )
                                        {
                                            isExists = true;
                                            break;
                                        }
                                    }
                                }
                                if (isExists)
                                {
                                    ShowMessage(Text.Message_TheSameRuleThatAlreadyExist);

                                }
                                else
                                {
                                    act.AddEndRule(this);
                                    if (this.IsTemporaryRule)
                                        //this.RuleName = Text.NewRule + _container.NextNewRuleIndex.ToString();
                                        this.RuleName = "";
                                    isLinked = true;
                                }


                            }
                        }

                    }
                }
            }


        }
        public void SimulateRulePointMouseLeftButtonUpEvent(RuleMoveType moveType, object sender, MouseButtonEventArgs e)
        {
            MoveType = moveType;
            Point_MouseLeftButtonUp(sender, e);
        }
        bool hadActualMove = false;
        public bool isTemplateRule = false;
        public bool IsTemporaryRule
        {
            get
            {
                return isTemplateRule;
            }
            set
            {
                isTemplateRule = value;
                if (value)
                {
                    DoubleCollection d = new DoubleCollection();
                    d.Add(1);
                    line.StrokeDashArray = d;
                }
                else
                    line.StrokeDashArray = null;

            }
        }
        bool pointHadActualMove = false;
        private void Point_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_container.IsMouseSelecting || _container.CurrentTemporaryRule != null)
            {
                e.Handled = false;
            }
            else
                e.Handled = true;

            trackingPointMouseMove = false;

            if (!_container.CurrentSelectedControlCollection.Contains(this))
            {
                FrameworkElement element = sender as FrameworkElement;
                element.ReleaseMouseCapture();

                mousePosition.X = mousePosition.Y = 0;
                element.Cursor = null;

                Point centerPoint = new Point();

                Activity a = null;

                if (MoveType == RuleMoveType.Begin)
                {
                    centerPoint.X = BeginPointPosition.X + beginPointRadius;
                    centerPoint.Y = BeginPointPosition.Y + beginPointRadius;
                    if (BeginActivity != null)
                        a = BeginActivity;
                }
                if (MoveType == RuleMoveType.End)
                {
                    centerPoint.X = EndPointPosition.X + endPointRadius;
                    centerPoint.Y = EndPointPosition.Y + endPointRadius;
                    if (EndActivity != null)
                        a = EndActivity;
                }


                if (a != null)
                {


                    //移去原来的关联
                    if (!a.PointIsInside(centerPoint))
                    {
                        if (MoveType == RuleMoveType.Begin
                             && BeginActivity != null)
                        {
                            BeginActivity = null;
                            a.RemoveBeginRule(this);

                        }
                        if (MoveType == RuleMoveType.End
                           && EndActivity != null)
                        {
                            EndActivity = null;
                            a.RemoveEndRule(this);

                        }
                    }
                }

                setLinkToActivity(MoveType);


                if (pointHadActualMove)
                    ruleChange();
            }
            if (!pointHadActualMove && !_container.IsMouseSelecting)
            {
                IsSelectd = !IsSelectd;
                _container.SetWorkFlowElementSelected(this, IsSelectd);
            }
        }
        public void SetRulePosition(Point beginPoint, Point endPoint)
        {
            SetRulePosition(beginPoint, endPoint, null, null);
        }
        public void SetRulePosition(Point beginPoint, Point endPoint, Point? turnPoint1, Point? turnPoint2)
        {
            if (double.IsNaN(beginPoint.X)
                || double.IsNaN(beginPoint.Y)
                || double.IsNaN(endPoint.X)
                || double.IsNaN(endPoint.Y)
                )
                return;


            begin.SetValue(Canvas.LeftProperty, beginPoint.X);
            begin.SetValue(Canvas.TopProperty, beginPoint.Y);

            end.SetValue(Canvas.LeftProperty, endPoint.X);
            end.SetValue(Canvas.TopProperty, endPoint.Y);



            Point p1 = new Point(beginPoint.X + beginPointRadius, beginPoint.Y + beginPointRadius);
            Point p4 = new Point(endPoint.X + endPointRadius, endPoint.Y + endPointRadius);

            Point p2 = new Point();
            Point p3 = new Point();

            if (LineType == RuleLineType.Line)
            {
                p2 = p1;
                p3 = p1;

                if (ruleTurnPoint1 != null)
                    ruleTurnPoint1.Visibility = Visibility.Collapsed;

                if (ruleTurnPoint2 != null)
                    ruleTurnPoint2.Visibility = Visibility.Collapsed;
            }
            else
            {

                RuleTurnPoint1.Visibility = Visibility.Visible;
                RuleTurnPoint2.Visibility = Visibility.Visible;
                if (turnPoint1 != null && turnPoint2 != null)
                {

                    RuleTurnPoint1.CenterPosition = turnPoint1.Value;
                    RuleTurnPoint2.CenterPosition = turnPoint2.Value;
                    p2 = RuleTurnPoint1.CenterPosition;
                    p3 = RuleTurnPoint2.CenterPosition;
                }
                else
                {

                    if (!trackingLineMouseMove)
                    {
                        if (TurnPoint1HadMoved)
                        {
                            p2 = RuleTurnPoint1.CenterPosition;
                        } if (TurnPoint2HadMoved)
                        {
                            p3 = RuleTurnPoint2.CenterPosition;
                        }
                    }


                    RuleTurnPoint1.CenterPosition = p2;
                    RuleTurnPoint2.CenterPosition = p3;
                }

            }

            line.Points.Clear();
            line.Points.Add(p1);
            line.Points.Add(p2);
            line.Points.Add(p3);
            line.Points.Add(p4);


            endArrow.SetAngleByPoint(p3, p4);
            setRuleNameControlPosition();

        }


        void ruleTurnPoint1_RuleTurnPointMove(object sender, MouseEventArgs e, Point newPoint)
        {
            // line.Points.Clear();
            positionIsChange = true;
            onRuleTurn1PointMove(newPoint);
        }

        void onRuleTurn1PointMove(Point newPoint)
        {
            TurnPoint1HadMoved = true;
            line.Points[1] = newPoint;
            if (BeginActivity != null)
            {
                this.BeginPointPosition = this.GetResetPoint(RuleTurnPoint1.CenterPosition, BeginActivity.CenterPoint, BeginActivity, RuleMoveType.Begin);

            }
            setRuleNameControlPosition();
        }

        void ruleTurnPoint2_RuleTurnPointMove(object sender, MouseEventArgs e, Point newPoint)
        {
            // line.Points.Clear(); 
            positionIsChange = true;

            onRuleTurn2PointMove(newPoint);
        }
        void onRuleTurn2PointMove(Point newPoint)
        {
            line.Points[2] = newPoint;
            TurnPoint2HadMoved = true;

            if (EndActivity != null)
            {
                this.EndPointPosition = this.GetResetPoint(RuleTurnPoint2.CenterPosition, EndActivity.CenterPoint, EndActivity, RuleMoveType.End);

            }
            endArrow.SetAngleByPoint(line.Points[2], line.Points[3]);

            setRuleNameControlPosition();
        }

        RuleTurnPoint ruleTurnPoint2;
        public RuleTurnPoint RuleTurnPoint2
        {
            get
            {
                if (ruleTurnPoint2 == null)
                {
                    ruleTurnPoint2 = new RuleTurnPoint();
                    // ruleTurnPoint2.Visibility = Visibility.Collapsed;

                    cnRuleContainer.Children.Add(ruleTurnPoint2);
                    ruleTurnPoint2.SetValue(Canvas.ZIndexProperty, 200);

                    ruleTurnPoint2.RuleTurnPointMove += new RuleTurnPoint.RuleTurnPointMoveDelegate(ruleTurnPoint2_RuleTurnPointMove);
                    ruleTurnPoint2.OnDoubleClick += new RuleTurnPoint.DoubleClickDelegate(ruleTurnPoint2_OnDoubleClick);


                }
                return ruleTurnPoint2;
            }
        }


        RuleTurnPoint ruleTurnPoint1;
        public RuleTurnPoint RuleTurnPoint1
        {
            get
            {
                if (ruleTurnPoint1 == null)
                {
                    ruleTurnPoint1 = new RuleTurnPoint();
                    // ruleTurnPoint1.Visibility = Visibility.Collapsed;
                    cnRuleContainer.Children.Add(ruleTurnPoint1);
                    ruleTurnPoint1.SetValue(Canvas.ZIndexProperty, 200);

                    ruleTurnPoint1.RuleTurnPointMove += new RuleTurnPoint.RuleTurnPointMoveDelegate(ruleTurnPoint1_RuleTurnPointMove);
                    ruleTurnPoint1.OnDoubleClick += new RuleTurnPoint.DoubleClickDelegate(ruleTurnPoint1_OnDoubleClick);




                }
                return ruleTurnPoint1;
            }
        }

        void ruleTurnPoint1_OnDoubleClick(object sender, EventArgs e)
        {
            Point p = new Point();
            p.X = (BeginPointPosition.X + ruleTurnPoint2.CenterPosition.X) / 2;
            p.Y = (BeginPointPosition.Y + ruleTurnPoint2.CenterPosition.Y) / 2;
            SetRulePosition(BeginPointPosition, EndPointPosition, p, ruleTurnPoint2.CenterPosition);
        }
        void ruleTurnPoint2_OnDoubleClick(object sender, EventArgs e)
        {
            Point p = new Point();
            p.X = (EndPointPosition.X + ruleTurnPoint1.CenterPosition.X) / 2;
            p.Y = (EndPointPosition.Y + ruleTurnPoint1.CenterPosition.Y) / 2;
            SetRulePosition(BeginPointPosition, EndPointPosition, ruleTurnPoint1.CenterPosition, p);
        }
        void setTurnPointInitPosition()
        {
            Point p2 = new Point();
            Point p3 = new Point();
            Point p1 = new Point(BeginPointPosition.X + beginPointRadius, BeginPointPosition.Y + beginPointRadius);
            Point p4 = new Point(EndPointPosition.X + endPointRadius, EndPointPosition.Y + endPointRadius);

            if (p4.X >= p1.X)
            {
                p2.X = p1.X + (p4.X - p1.X) / 2;
                p2.Y = p1.Y;

                p3.X = p2.X;
                p3.Y = p4.Y;
            }
            else
            {
                p2.X = p1.X - (p1.X - p4.X) / 2;
                p2.Y = p1.Y;

                p3.X = p2.X;
                p3.Y = p4.Y;
            }
            RuleTurnPoint1.CenterPosition = p2;
            RuleTurnPoint2.CenterPosition = p3;
            IsSelectd = IsSelectd;
        }

        public bool TurnPoint1HadMoved = false;
        public bool TurnPoint2HadMoved = false;
        public void SetPositionByDisplacement(double x, double y)
        {
            if (BeginActivity != null && EndActivity != null
                            && !_container.CurrentSelectedControlCollection.Contains(BeginActivity)
                            && !_container.CurrentSelectedControlCollection.Contains(EndActivity)
                            )
            {
            }
            else if (BeginActivity != null && EndActivity == null
                           && !_container.CurrentSelectedControlCollection.Contains(BeginActivity)
                            )
            {
                SetRulePositionByDisplacement(x, y, RuleMoveType.End);

            }
            else if (BeginActivity == null && EndActivity != null
                   && !_container.CurrentSelectedControlCollection.Contains(EndActivity)
                    )
            {
                SetRulePositionByDisplacement(x, y, RuleMoveType.Begin);

            }
            else
            {
                SetRulePositionByDisplacement(x, y, RuleMoveType.Line);

            }
        }
        public void SetRulePositionByDisplacement(double x, double y, RuleMoveType moveType)
        {
            Point beginPoint = BeginPointPosition;
            Point endPoint = EndPointPosition;


            if (moveType == RuleMoveType.Begin || moveType == RuleMoveType.Line)
            {
                beginPoint.X += x;
                beginPoint.Y += y;
            }

            if (moveType == RuleMoveType.End || moveType == RuleMoveType.Line)
            {
                endPoint.X += x;
                endPoint.Y += y;
            }

            if (LineType == RuleLineType.Line)
            {
                SetRulePosition(beginPoint, endPoint);

            }
            else
            {
                SetRulePosition(beginPoint, endPoint,
                    new Point(RuleTurnPoint1.CenterPosition.X + x, RuleTurnPoint1.CenterPosition.Y + y),
                    new Point(RuleTurnPoint2.CenterPosition.X + x, RuleTurnPoint2.CenterPosition.Y + y));

            }

        }

        bool isSelectd = false;
        public bool IsSelectd
        {
            get
            {
                return isSelectd;

            }
            set
            {
                isSelectd = value;
                if (isSelectd)
                {
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Color.FromArgb(255, 255, 181, 0);
                    begin.Fill = brush;
                    endArrow.Stroke = brush;
                    line.Stroke = brush;
                    if (!_container.CurrentSelectedControlCollection.Contains(this))
                        _container.AddSelectedControl(this);
                    if (LineType == RuleLineType.Polyline)
                    {
                        ruleTurnPoint1.Fill = brush;
                        ruleTurnPoint2.Fill = brush;
                    }

                }
                else
                {
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Color.FromArgb(255, 0, 128, 0);
                    begin.Fill = brush;
                    endArrow.Stroke = brush;
                    line.Stroke = brush;
                    if (LineType == RuleLineType.Polyline)
                    {
                        ruleTurnPoint1.Fill = brush;
                        ruleTurnPoint2.Fill = brush;
                    }
                }
            }

        }
        bool trackingPointMouseMove = false;
        private void Point_MouseMove(object sender, MouseEventArgs e)
        {
            if (trackingPointMouseMove)
            {
                if (e.GetPosition(null) == mousePosition)
                    return;
                if (_container.CurrentSelectedControlCollection.Contains(this))
                {
                    if (MoveType == RuleMoveType.Begin && BeginActivity != null
                        && !_container.CurrentSelectedControlCollection.Contains(this.BeginActivity))
                    {
                        pointHadActualMove = false;
                        mousePosition = e.GetPosition(null);
                        return;
                    }
                    if (MoveType == RuleMoveType.End && EndActivity != null
                        && !_container.CurrentSelectedControlCollection.Contains(this.EndActivity))
                    {
                        pointHadActualMove = false;
                        mousePosition = e.GetPosition(null);
                        return;
                    }
                }

                FrameworkElement element = sender as FrameworkElement;
                Point currentPoint = e.GetPosition(this);

                double deltaV = e.GetPosition(null).Y - mousePosition.Y;
                double deltaH = e.GetPosition(null).X - mousePosition.X;

                double containerWidth = (double)this.Parent.GetValue(Canvas.WidthProperty);
                double containerHeight = (double)this.Parent.GetValue(Canvas.HeightProperty);

                if (currentPoint.X > containerWidth
                   || currentPoint.Y > containerHeight
                    || currentPoint.X < 0
                    || currentPoint.Y < 0
                    )
                {
                    //超过流程容器的范围


                }
                else
                {

                    positionIsChange = true;
                    if (_container.CurrentSelectedControlCollection.Contains(this))
                    {
                        SetPositionByDisplacement(deltaH, deltaV);

                    }
                    else
                    {

                        if (MoveType == RuleMoveType.Begin)
                        {
                            this.BeginPointPosition = currentPoint;

                            if (EndActivity != null)
                            {
                                if (LineType == RuleLineType.Line)
                                {
                                    this.EndPointPosition = this.GetResetPoint(currentPoint, EndActivity.CenterPoint, EndActivity, RuleMoveType.End);
                                }
                                else
                                {
                                    this.EndPointPosition = this.GetResetPoint(RuleTurnPoint2.CenterPosition, EndActivity.CenterPoint, EndActivity, RuleMoveType.End);

                                }
                            }


                        }
                        else if (MoveType == RuleMoveType.End)
                        {
                            this.EndPointPosition = currentPoint;

                            if (BeginActivity != null)
                            {
                                if (LineType == RuleLineType.Line)
                                {
                                    this.BeginPointPosition = this.GetResetPoint(currentPoint, BeginActivity.CenterPoint, BeginActivity, RuleMoveType.Begin);
                                }
                                else
                                {
                                    this.BeginPointPosition = this.GetResetPoint(RuleTurnPoint1.CenterPosition, BeginActivity.CenterPoint, BeginActivity, RuleMoveType.Begin);

                                }
                            }
                        }
                    }

                }
                pointHadActualMove = true;
                _container.MoveControlCollectionByDisplacement(deltaH, deltaV, this);
                mousePosition = e.GetPosition(null);
            }

        }

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            hadActualMove = false;
            trackingLineMouseMove = false;
            if (_doubleClickTimer.IsEnabled)
            {
                _doubleClickTimer.Stop();
                //spContentMenu.Visibility = Visibility.Collapsed;
                _container.ShowRuleSetting(this);

            }
            else
            {
                _doubleClickTimer.Start();
                this.SetValue(Canvas.ZIndexProperty, _container.NextMaxIndex);
                FrameworkElement element = sender as FrameworkElement;
                mousePosition = e.GetPosition(null);
                if (null != element)
                {
                    trackingLineMouseMove = true;
                    element.CaptureMouse();
                    element.Cursor = Cursors.Hand;

                }
            }
            e.Handled = true;

        }

        private void Line_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_container.IsMouseSelecting || _container.CurrentTemporaryRule != null)
            {
                e.Handled = false;

            }
            else
                e.Handled = true;

            if (!hadActualMove && !_container.IsMouseSelecting)
            {
                IsSelectd = !IsSelectd;
                _container.SetWorkFlowElementSelected(this, IsSelectd);
            }
            trackingLineMouseMove = false;
            FrameworkElement element = sender as FrameworkElement;
            element.ReleaseMouseCapture();

            mousePosition.X = mousePosition.Y = 0;
            element.Cursor = null;

            setLinkToActivity(RuleMoveType.Begin);
            setLinkToActivity(RuleMoveType.End);

            if (hadActualMove)
                ruleChange();
        }

        private void Line_MouseMove(object sender, MouseEventArgs e)
        {


            if (trackingLineMouseMove)
            {
                FrameworkElement element = sender as FrameworkElement;


                if (BeginActivity != null && EndActivity != null
                    && !_container.CurrentSelectedControlCollection.Contains(BeginActivity)
                    && !_container.CurrentSelectedControlCollection.Contains(EndActivity)
                    )
                    return;
                if (mousePosition == e.GetPosition(null))
                    return;
                hadActualMove = true;

                double deltaV = e.GetPosition(null).Y - mousePosition.Y;
                double deltaH = e.GetPosition(null).X - mousePosition.X;

                double newLeft = e.GetPosition((FrameworkElement)this.Parent).X;
                double newTop = e.GetPosition((FrameworkElement)this.Parent).Y;

                double containerWidth = (double)this.Parent.GetValue(Canvas.WidthProperty);
                double containerHeight = (double)this.Parent.GetValue(Canvas.HeightProperty);

                if (containerWidth < newLeft || containerWidth < newTop
                    || newLeft < 0 || newTop < 0
                    )
                {
                    //超过流程容器的范围

                }

                else
                {
                    positionIsChange = true;

                    SetPositionByDisplacement(deltaH, deltaV);
                    _container.MoveControlCollectionByDisplacement(deltaH, deltaV, this);
                }
                mousePosition = e.GetPosition(null);
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            canShowMenu = true;
            ttRuleTip.Content = RuleData.RuleName + "\r\n" + RuleData.RuleCondition;

        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            canShowMenu = false;
        }
    }
}
