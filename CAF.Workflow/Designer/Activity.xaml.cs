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
using Shareidea.Web.UI.Control.Workflow.Designer.Component;
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

    public delegate void MoveDelegate(Activity a, MouseEventArgs e);
    public delegate void DeleteDelegate(Activity a);

    public delegate void ActivityChangeDelegate(Activity a);



    public partial class Activity : UserControl, IElement
    {
        //
        double origPictureWidth = 0;
        double origPictureHeight = 0;
        Point origPosition;
        bool positionIsChange = true;
        public void Zoom(double zoomDeep)
        {
            if (origPictureWidth == 0)
            {
                origPictureWidth = sdPicture.PictureWidth;
                origPictureHeight = sdPicture.PictureHeight;
            }
            if (positionIsChange)
            {
                origPosition = this.Position;
                positionIsChange = false;
            }

            sdPicture.PictureHeight = origPictureHeight * zoomDeep;
            sdPicture.PictureWidth = origPictureWidth * zoomDeep;
            this.Position = new Point(origPosition.X * zoomDeep, origPosition.Y * zoomDeep);

        }


        public double PictureWidth
        {
            get
            {
                return sdPicture.PictureWidth;
            }
        }
        public double PictureHeight
        {
            get
            {
                return sdPicture.PictureHeight;
            }
        }

        bool isPassCheck
        {
            set
            {
                if (value)
                {

                    sdPicture.ResetInitColor();
                }
                else
                {
                    sdPicture.SetWarningColor();
                }
            }
        }
        public Point GetPointOfIntersection(Point beginPoint, Point endPoint, RuleMoveType type)
        {


            double endPointRadius = 4;
            double beginPointRadius = 4;
            Point p = new Point();
            if (Type == ActivityType.INTERACTION
                 || Type == ActivityType.AND_MERGE
                || Type == ActivityType.OR_MERGE
                || Type == ActivityType.VOTE_MERGE
                )
            {

                #region


                if (Math.Abs(endPoint.X - beginPoint.X) <= PictureWidth / 2
                    && Math.Abs(endPoint.Y - beginPoint.Y) <= PictureHeight / 2)
                {
                    p = endPoint;
                }
                else
                {
                    //起始点坐标和终点坐标之间的夹角（相对于Y轴坐标系）
                    double angle = Math.Abs(Math.Atan((endPoint.X - beginPoint.X) / (endPoint.Y - beginPoint.Y)) * 180.0 / Math.PI);
                    //活动的长和宽之间的夹角（相对于Y轴坐标系）
                    double angel2 = Math.Abs(Math.Atan(PictureWidth / PictureHeight) * 180.0 / Math.PI);
                    //半径
                    double radio = PictureHeight < PictureWidth ? PictureHeight / 2 : PictureWidth / 2;

                    if (angle <= angel2)//起始点坐标在终点坐标的上方,或者下方
                    {
                        if (endPoint.Y < beginPoint.Y)//在上方
                        {
                            if (endPoint.X < beginPoint.X)
                                p.X = endPoint.X + Math.Tan(Math.PI * angle / 180.0) * radio;
                            else
                                p.X = endPoint.X - Math.Tan(Math.PI * angle / 180.0) * radio;

                            p.Y = endPoint.Y + PictureHeight / 2;
                        }
                        else//在下方
                        {
                            if (endPoint.X < beginPoint.X)
                                p.X = endPoint.X + Math.Tan(Math.PI * angle / 180.0) * radio;
                            else
                                p.X = endPoint.X - Math.Tan(Math.PI * angle / 180.0) * radio;

                            p.Y = endPoint.Y - PictureHeight / 2;
                        }

                    }

                    else//左方或者右方
                    {
                        if (endPoint.X < beginPoint.X)//在右方
                        {
                            p.X = endPoint.X + PictureWidth / 2;
                            if (endPoint.Y < beginPoint.Y)
                                p.Y = endPoint.Y + Math.Tan(Math.PI * (90 - angle) / 180.0) * radio;
                            else
                                p.Y = endPoint.Y - Math.Tan(Math.PI * (90 - angle) / 180.0) * radio;
                        }
                        else//在左方
                        {
                            p.X = endPoint.X - PictureWidth / 2;
                            if (endPoint.Y < beginPoint.Y)
                                p.Y = endPoint.Y + Math.Tan(Math.PI * (90 - angle) / 180.0) * radio;
                            else
                                p.Y = endPoint.Y - Math.Tan(Math.PI * (90 - angle) / 180.0) * radio;

                        }
                    }
                }




                if (type == RuleMoveType.End)
                {
                    p.X -= endPointRadius;
                    p.Y -= endPointRadius;
                }
                if (type == RuleMoveType.Begin)
                {
                    p.X -= beginPointRadius;
                    p.Y -= beginPointRadius;
                }

                #endregion

            }
            else if (Type == ActivityType.INITIAL
              || Type == ActivityType.COMPLETION
              || Type == ActivityType.AUTOMATION
               || Type == ActivityType.DUMMY
                || Type == ActivityType.SUBPROCESS)
            {
                #region
                if (Math.Abs(endPoint.X - beginPoint.X) <= PictureWidth / 2
                    && Math.Abs(endPoint.Y - beginPoint.Y) <= PictureHeight / 2)
                {
                    p = endPoint;
                }
                else
                {
                    double radial = (PictureWidth < PictureHeight ? PictureWidth : PictureHeight) / 2;
                    double bc = Math.Sqrt((endPoint.X - beginPoint.X) * (endPoint.X - beginPoint.X) + (endPoint.Y - beginPoint.Y) * (endPoint.Y - beginPoint.Y));
                    p.X = endPoint.X - (endPoint.X - beginPoint.X) * radial / bc;
                    p.Y = endPoint.Y - (endPoint.Y - beginPoint.Y) * radial / bc;


                }
                if (type == RuleMoveType.End)
                {
                    p.X -= endPointRadius;
                    p.Y -= endPointRadius;
                }
                if (type == RuleMoveType.Begin)
                {
                    p.X -= beginPointRadius;
                    p.Y -= beginPointRadius;
                }


                #endregion

            }

            else if (Type == ActivityType.AND_BRANCH
                 || Type == ActivityType.OR_BRANCH
               )
            {
                if (Math.Abs(endPoint.X - beginPoint.X) <= PictureWidth / 2
                    && Math.Abs(endPoint.Y - beginPoint.Y) <= PictureHeight / 2)
                {
                    p = endPoint;

                    if (type == RuleMoveType.End)
                    {
                        p.X -= endPointRadius;
                        p.Y -= endPointRadius;
                    }
                    if (type == RuleMoveType.Begin)
                    {
                        p.X -= beginPointRadius;
                        p.Y -= beginPointRadius;
                    }
                }
                else
                {

                    //double angle = Math.Abs(Math.Atan((endPoint.X - beginPoint.X) / (endPoint.Y - beginPoint.Y)) * 180.0 / Math.PI);
                    //if (angle < 45)
                    //{
                    //    if (endPoint.Y < beginPoint.Y)
                    //    {
                    //        p = ThisPointCollection[2];
                    //    }
                    //    else
                    //    {
                    //        p = ThisPointCollection[0];

                    //    }

                    //}

                    //else
                    //{
                    //    if (endPoint.X < beginPoint.X)
                    //    {
                    //        p = ThisPointCollection[1];

                    //    }
                    //    else
                    //    {
                    //        p = ThisPointCollection[3];

                    //    }
                    //}


                    double x = 0, y = 0;
                    double tan = Math.Abs((endPoint.Y - beginPoint.Y) / (beginPoint.X - endPoint.X));

                    if (endPoint.X <= beginPoint.X && endPoint.Y >= beginPoint.Y)//右上
                    {
                        y = (endPoint.Y + (ThisPointCollection[0].Y + (double)GetValue(Canvas.TopProperty)) * tan) / (1 + tan);

                        x = (endPoint.Y - y) / tan + endPoint.X;


                    }
                    else if (this.CenterPoint.X <= beginPoint.X && this.CenterPoint.Y <= beginPoint.Y)//右下
                    {
                        y = (endPoint.Y + (ThisPointCollection[2].Y + (double)GetValue(Canvas.TopProperty)) * tan) / (1 + tan);
                        x = (y - endPoint.Y) / tan + endPoint.X;



                    }
                    else if (this.CenterPoint.X >= beginPoint.X && this.CenterPoint.Y >= beginPoint.Y)//左上
                    {
                        y = (endPoint.Y + (ThisPointCollection[0].Y + (double)GetValue(Canvas.TopProperty)) * tan) / (1 + tan);

                        x = (y - endPoint.Y) / tan + endPoint.X;



                    }
                    else if (this.CenterPoint.X >= beginPoint.X && this.CenterPoint.Y <= beginPoint.Y)//左下
                    {
                        y = (endPoint.Y + (ThisPointCollection[2].Y + (double)GetValue(Canvas.TopProperty)) * tan) / (1 + tan);
                        x = (endPoint.Y - y) / tan + endPoint.X;



                    }
                    p.Y = y;
                    p.X = x;
                    if (type == RuleMoveType.End)
                    {
                        p.X += -endPointRadius;
                        p.Y += -endPointRadius;
                    }
                    if (type == RuleMoveType.Begin)
                    {
                        p.X += -beginPointRadius;
                        p.Y += -beginPointRadius;
                    }
                }



            }


            return p;
        }
        public CheckResult CheckSave()
        {
            CheckResult cr = new CheckResult();
            cr.IsPass = true;



            if (Type == ActivityType.INITIAL)
            {
                if (EndRuleCollections != null
                    && EndRuleCollections.Count > 0)
                {
                    cr.IsPass = false;
                    cr.Message += string.Format(Text.Message_CanNotHavePreactivity, ActivityName);
                }
                if (BeginRuleCollections == null
                    || BeginRuleCollections.Count == 0)
                {
                    cr.IsPass = false;//必须至少有一个后继活动
                    cr.Message += string.Format(Text.Message_MustHaveAtLeastOneFollowUpActivity, ActivityName);
                }
            }
            else if (Type == ActivityType.COMPLETION)
            {
                if (BeginRuleCollections != null
                    && BeginRuleCollections.Count > 0)
                {
                    cr.IsPass = false;//不能有后继活动
                    cr.Message += string.Format(Text.Message_NotHaveFollowUpActivity, ActivityName);
                }
                if (EndRuleCollections == null
                    || EndRuleCollections.Count == 0)
                {
                    cr.IsPass = false;//必须至少有一个前驱活动
                    cr.Message += string.Format(Text.Message_MustHaveAtLeastOnePreactivity, ActivityName);
                }
            }
            else
            {
                if ((BeginRuleCollections == null
                || BeginRuleCollections.Count == 0)
                    && (EndRuleCollections == null
                || EndRuleCollections.Count == 0))
                {
                    cr.IsPass = false;//必须设置前驱和后继活动
                    cr.Message += string.Format(Text.Message_RequireTheInstallationOfPreAndFollowupActivity, ActivityName);
                }
                else
                {

                    if (BeginRuleCollections == null
                    || BeginRuleCollections.Count == 0)
                    {
                        cr.IsPass = false;//必须至少有一个后继活动
                        cr.Message += string.Format(Text.Message_MustHaveAtLeastOneFollowUpActivity, ActivityName);
                    }
                    if (EndRuleCollections == null
                    || EndRuleCollections.Count == 0)
                    {
                        cr.IsPass = false;//必须至少有一个前驱活动
                        cr.Message += string.Format(Text.Message_MustHaveAtLeastOnePreactivity, ActivityName);
                    }
                    if (Type == ActivityType.AND_BRANCH
                        || Type == ActivityType.OR_BRANCH)
                    {
                        if (EndRuleCollections != null
                            && EndRuleCollections.Count > 1)
                        {
                            //cr.IsPass = false;//有且只能有一个前驱活动
                            //cr.Message += string.Format(Text.Message_MustHaveOnlyOnePreactivity, ActivityName);
                        }
                    }

                    if (Type == ActivityType.AND_MERGE
                        || Type == ActivityType.OR_MERGE
                        || Type == ActivityType.VOTE_MERGE)
                    {
                        if (BeginRuleCollections != null
                            && BeginRuleCollections.Count > 1)
                        {
                            //cr.IsPass = false;
                            // cr.Message += string.Format(Text.Message_MustHaveOnlyOneFollowUpActivity, ActivityName);
                        }
                    }
                }
            }
            isPassCheck = cr.IsPass;
            if (!cr.IsPass)
            {
                errorTipControl.Visibility = Visibility.Visible;
                errorTipControl.ErrorMessage = cr.Message.TrimEnd("\r\n".ToCharArray());
            }
            else
            {
                if (_errorTipControl != null)
                {
                    _errorTipControl.Visibility = Visibility.Collapsed;
                    container.Children.Remove(_errorTipControl);
                    _errorTipControl = null;
                }
            }
            return cr;


        }
        ErrorTip _errorTipControl;
        ErrorTip errorTipControl
        {
            get
            {
                if (_errorTipControl == null)
                {
                    _errorTipControl = new ErrorTip();
                    _errorTipControl.ParentElement = this;
                    container.Children.Add(_errorTipControl);

                }
                _errorTipControl.SetValue(Canvas.ZIndexProperty, 1);
                _errorTipControl.SetValue(Canvas.TopProperty, -this.PictureHeight / 2);
                _errorTipControl.SetValue(Canvas.LeftProperty, this.PictureWidth);
                return _errorTipControl;
            }
        }


        public MergePictureRepeatDirection RepeatDirection
        {
            get
            {

                return sdPicture.RepeatDirection;
            }
            set
            {

                bool isChanged = false;
                if (sdPicture.RepeatDirection != value)
                {

                    isChanged = true;

                }
                sdPicture.RepeatDirection = value;

                if (isChanged)
                {
                    Move(this, null);
                }
            }
        }



        ActivityType type = ActivityType.INTERACTION;
        public ActivityType Type
        {
            get
            {
                return type;
            }
            set
            {
                bool isChanged = false;
                if (type != value)
                {

                    isChanged = true;
                }
                type = value;
                if (type == ActivityType.COMPLETION)
                {
                    eiCenterEllipse.Visibility = Visibility.Collapsed;

                }
                else
                {
                    eiCenterEllipse.Visibility = Visibility.Visible;
                }
                sdPicture.Type = type;
                if (isChanged)
                    Move(this, null);

            }
        }

        public void SetActivityData(ActivityComponent activityData)
        {
            bool isChanged = false;


            if (ActivityData.ActivityName != activityData.ActivityName
                || ActivityData.ActivityType != activityData.ActivityType
                || ActivityData.RepeatDirection != activityData.RepeatDirection
                || ActivityData.ActivityPost != activityData.ActivityPost)
            {
                isChanged = true;

            }

            ActivityData = activityData;
            setUIValueByActivityData(activityData);
            if (isChanged)
            {
                if (ActivityChanged != null)
                    ActivityChanged(this);
            }
            IsSelectd = IsSelectd;

        }

        void setUIValueByActivityData(ActivityComponent activityData)
        {
            sdPicture.AcitivtyName = activityData.ActivityName;
            ActivityType type = (ActivityType)Enum.Parse(typeof(ActivityType), activityData.ActivityType, true);
            MergePictureRepeatDirection repeatDirection = (MergePictureRepeatDirection)Enum.Parse(typeof(MergePictureRepeatDirection), activityData.RepeatDirection, true);
            Type = type;
            RepeatDirection = repeatDirection;
            SubFlow = activityData.SubFlow;
            ActivityPost = activityData.ActivityPost;

        }
        string _subFlow;
        public string SubFlow
        {
            get
            {
                return _subFlow;
            }
            set
            {
                _subFlow = value;
            }
        }


        public PointCollection ThisPointCollection
        {
            get
            {
                return sdPicture.ThisPointCollection;
            }
        }
        ActivityComponent getActivityComponentFromServer(string activityID)
        {
            ActivityComponent ac = new ActivityComponent();
            ac = new ActivityComponent();
            ac.ActivityID = this.ActivityID;
            ac.UniqueID = this.UniqueID;
            ac.ActivityName = sdPicture.AcitivtyName;
            ac.ActivityPost = this.ActivityPost;
            ac.ActivityType = Type.ToString();
            ac.SubFlow = this.SubFlow;
            return ac;
        }
        ActivityComponent activityData;
        public ActivityComponent ActivityData
        {
            get
            {
                if (activityData == null)
                {
                    if (EditType == PageEditType.Add)
                    {
                        activityData = new ActivityComponent();
                        activityData.ActivityID = this.ActivityID;
                        activityData.UniqueID = this.UniqueID;
                        activityData.ActivityName = sdPicture.AcitivtyName;
                        activityData.ActivityPost = this.ActivityPost;
                        activityData.ActivityType = Type.ToString();
                        activityData.RepeatDirection = RepeatDirection.ToString();
                        activityData.SubFlow = SubFlow;


                    }
                    else if (EditType == PageEditType.Modify)
                    {
                        activityData = getActivityComponentFromServer(this.ActivityID);

                    }
                }
                return activityData;
            }
            set
            {
                activityData = value;
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

        public event ActivityChangeDelegate ActivityChanged;

        public void UpperZIndex()
        {
            ZIndex = _container.NextMaxIndex;
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

        public Point CenterPoint
        {
            get
            {


                return new Point((double)this.GetValue(Canvas.LeftProperty) + this.Width / 2, (double)this.GetValue(Canvas.TopProperty) + this.Height / 2);

            }
            set
            {


                this.SetValue(Canvas.LeftProperty, value.X - this.Width / 2);
                this.SetValue(Canvas.TopProperty, value.Y - this.Height / 2);
                Move(this, null);


            }
        }

        public Point Position
        {
            get
            {
                Point position;

                position = new Point();
                position.Y = (double)this.GetValue(Canvas.TopProperty);
                position.X = (double)this.GetValue(Canvas.LeftProperty);


                return position;
            }
            set
            {

                this.SetValue(Canvas.TopProperty, value.Y);
                this.SetValue(Canvas.LeftProperty, value.X);
                Move(this, null);
            }
        }
        public WorkFlowElementType ElementType
        {
            get
            {
                return WorkFlowElementType.Activity;
            }
        }
        public string ToXmlString()
        {
            System.Text.StringBuilder xml = new System.Text.StringBuilder();
            xml.Append(@"       <Activity ");
            xml.Append(@" UniqueID=""" + UniqueID + @"""");
            xml.Append(@" ActivityID=""" + ActivityID + @"""");
            xml.Append(@" ActivityName=""" + ActivityName + @"""");
            xml.Append(@" ActivityPost=""" + ActivityPost + @"""");
            xml.Append(@" Type=""" + Type.ToString() + @"""");
            xml.Append(@" SubFlow=""" + (Type == ActivityType.SUBPROCESS ? SubFlow : @"") + @"""");
            xml.Append(@" PositionX=""" + CenterPoint.X + @"""");
            xml.Append(@" PositionY=""" + CenterPoint.Y + @"""");
            xml.Append(@" RepeatDirection=""" + RepeatDirection.ToString() + @"""");
            xml.Append(@" ZIndex=""" + ZIndex + @""">");

            xml.Append(Environment.NewLine);
            xml.Append("        </Activity>");

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
        public void AddBeginRule(Rule r)
        {
            if (!BeginRuleCollections.Contains(r))
            {
                BeginRuleCollections.Add(r);
                r.BeginActivity = this;
                Move(this, null);
            }

        }
        public void RemoveBeginRule(Rule r)
        {
            if (BeginRuleCollections.Contains(r))
            {
                BeginRuleCollections.Remove(r);
                r.RemoveBeginActivity(this);

            }
        }


        public void AddEndRule(Rule r)
        {
            if (!EndRuleCollections.Contains(r))
            {
                EndRuleCollections.Add(r);
                r.EndActivity = this;
                Move(this, null);

            }

        }
        public void RemoveEndRule(Rule r)
        {
            if (EndRuleCollections.Contains(r))
            {
                EndRuleCollections.Remove(r);
                r.RemoveEndActivity(this);
            }
        }

        public event MoveDelegate ActivityMove;
        public event DeleteDelegate DeleteActivity;

        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }
        }

        bool isDeleted = false;
        public void Delete()
        {


            if (!isDeleted)
            {
                isDeleted = true;
                canShowMenu = false;
                sbClose.Completed += new EventHandler(sbClose_Completed);
                sbClose.Begin();
            }

        }

        void sbClose_Completed(object sender, EventArgs e)
        {
            if (isDeleted)
            {
                if (this.BeginRuleCollections != null)
                {
                    foreach (Rule r in this.BeginRuleCollections)
                    {
                        r.RemoveBeginActivity(this);
                    }
                }
                if (this.EndRuleCollections != null)
                {
                    foreach (Rule r in this.EndRuleCollections)
                    {
                        r.RemoveEndActivity(this);
                    }
                }
                if (DeleteActivity != null)
                    DeleteActivity(this);

                _container.RemoveActivity(this);

                //if (ActivityChanged != null)
                //    ActivityChanged(this);
            }
        }

        public void Move(Activity a, MouseEventArgs e)
        {
            if (ActivityMove != null)
                ActivityMove(a, e);
        }

        Activity originActivity;
        public Activity OriginActivity
        {
            get
            {
                return originActivity;
            }
            set
            {
                originActivity = null;
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
        string activityID;
        public string ActivityID
        {
            get
            {

                return activityID;
            }
            set
            {
                activityID = value;
            }

        }
        public string ActivityName
        {
            get
            {
                return sdPicture.AcitivtyName;
            }
            set
            {
                sdPicture.AcitivtyName = value;
            }

        }
        public string ActivityPost
        {
            get
            {
                return sdPicture.ActivityPost;
            }
            set
            {
                sdPicture.ActivityPost = value;
            }

        }

        public System.Collections.Generic.List<Rule> BeginRuleCollections = new List<Rule>();
        public System.Collections.Generic.List<Rule> EndRuleCollections = new List<Rule>();
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

        public void ShowMessage(string message)
        {
            _container.ShowMessage(message);
        }
        System.Windows.Threading.DispatcherTimer _doubleClickTimer;
        public Activity(IContainer container, ActivityType at)
        {
            InitializeComponent();
            _container = container;
            editType = PageEditType.Add;
            this.Type = at;
            System.Windows.Browser.HtmlPage.Document.AttachEvent("oncontextmenu", OnContextMenu);
            this.Name = UniqueID;

            _doubleClickTimer = new System.Windows.Threading.DispatcherTimer();
            _doubleClickTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            _doubleClickTimer.Tick += new EventHandler(DoubleClick_Timer);
            sbDisplay.Begin();

        }

        void DoubleClick_Timer(object sender, EventArgs e)
        {
            _doubleClickTimer.Stop();
        }

        private void OnContextMenu(object sender, System.Windows.Browser.HtmlEventArgs e)
        {

            if (_container.MouseIsInContainer)
            {
                e.PreventDefault();

                if (canShowMenu && !IsDeleted)
                {

                    _container.ShowActivityContentMenu(this, sender, e);
                }
            }
        }

        bool trackingMouseMove = false;

        Point mousePosition;
        bool hadActualMove = false;
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {


            if (trackingMouseMove)
            {
                FrameworkElement element = sender as FrameworkElement;
                element.Cursor = Cursors.Hand;

                if (e.GetPosition(null) == mousePosition)
                    return;
                hadActualMove = true;
                double deltaV = e.GetPosition(null).Y - mousePosition.Y;
                double deltaH = e.GetPosition(null).X - mousePosition.X;
                double newTop = deltaV + Position.Y;
                double newLeft = deltaH + Position.X;




                double containerWidth = (double)this.Parent.GetValue(Canvas.WidthProperty);
                double containerHeight = (double)this.Parent.GetValue(Canvas.HeightProperty);
                if ((CenterPoint.X - sdPicture.PictureWidth / 2 < 2 && deltaH < 0)
                    || (CenterPoint.Y - sdPicture.PictureHeight / 2 < 2 && deltaV < 0)
                    )
                {
                    //超过流程容器的范围
                }
                else
                {
                    positionIsChange = true;
                    this.SetValue(Canvas.TopProperty, newTop);
                    this.SetValue(Canvas.LeftProperty, newLeft);

                    Move(this, e);
                    mousePosition = e.GetPosition(null);
                    _container.MoveControlCollectionByDisplacement(deltaH, deltaV, this);
                }


            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            hadActualMove = false;
            if (_doubleClickTimer.IsEnabled)
            {
                _doubleClickTimer.Stop();
                _container.ShowActivitySetting(this);

            }
            else
            {
                _doubleClickTimer.Start();
                this.SetValue(Canvas.ZIndexProperty, _container.NextMaxIndex);

                FrameworkElement element = sender as FrameworkElement;
                mousePosition = e.GetPosition(null);
                trackingMouseMove = true;
                if (null != element)
                {
                    element.CaptureMouse();
                    element.Cursor = Cursors.Hand;
                }
            }
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_container.IsMouseSelecting || _container.CurrentTemporaryRule != null)
            {
                e.Handled = false;
            }
            else
                e.Handled = true;

            canShowMenu = true;
            if (!hadActualMove && !_container.IsMouseSelecting)
            {


                IsSelectd = !IsSelectd;
                _container.SetWorkFlowElementSelected(this, IsSelectd);
            }
            FrameworkElement element = sender as FrameworkElement;
            trackingMouseMove = false;
            element.ReleaseMouseCapture();

            mousePosition.X = mousePosition.Y = 0;
            element.Cursor = null;

            if (hadActualMove)
                activityChange();
        }
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            canShowMenu = true;

            //ToolTip tt = new ToolTip();
            //ttActivityTip.Content = ActivityData.ActivityName + "\r\n" + ActivityData.ActivityType;
            return;

        }
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            canShowMenu = false;

        }
        public void SetPositionByDisplacement(double x, double y)
        {


            Point p = new Point();
            p.X = (double)this.GetValue(Canvas.LeftProperty);
            p.Y = (double)this.GetValue(Canvas.TopProperty);

            this.SetValue(Canvas.TopProperty, p.Y + y);
            this.SetValue(Canvas.LeftProperty, p.X + x);
            Move(this, null);

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
                    sdPicture.SetSelectedColor();

                    if (!_container.CurrentSelectedControlCollection.Contains(this))
                        _container.AddSelectedControl(this);



                }
                else
                {
                    sdPicture.ResetInitColor();
                }
            }

        }


        public bool PointIsInside(Point p)
        {
            bool isInside = false;


            double thisWidth = sdPicture.PictureWidth;
            double thisHeight = sdPicture.PictureHeight;

            double thisX = CenterPoint.X - thisWidth / 2;
            double thisY = CenterPoint.Y - thisHeight / 2;

            if (thisX < p.X && p.X < thisX + thisWidth
                && thisY < p.Y && p.Y < thisY + thisHeight)
            {
                isInside = true;
            }


            return isInside;
        }



        public Activity Clone()
        {
            Activity clone = new Activity(this._container, this.Type);
            clone.originActivity = this;
            clone.ActivityData = new ActivityComponent();
            clone.ActivityData.ActivityName = this.ActivityData.ActivityName;
            clone.activityData.ActivityPost = this.activityData.ActivityPost;
            clone.ActivityData.ActivityType = this.ActivityData.ActivityType;
            clone.setUIValueByActivityData(clone.ActivityData);
            // clone.CenterPoint = this.CenterPoint;
            clone.CenterPoint = this.CenterPoint;
            clone.ZIndex = this.ZIndex;
            //_container.AddActivity(clone);

            return clone;
        }

        void activityChange()
        {
            if (ActivityChanged != null)
                ActivityChanged(this);
        }

        private void CenterEllipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isDeleted)
                return;
            e.Handled = true;
            if (_doubleClickTimer.IsEnabled)
            {
                _doubleClickTimer.Stop();
                _container.ShowActivitySetting(this);

            }
            else
            {
                _doubleClickTimer.Start();

                if (!_container.CurrentSelectedControlCollection.Contains(this))
                {
                    if (_container.CurrentTemporaryRule == null)
                    {
                        if (this.Type != ActivityType.COMPLETION)
                        {
                            _container.CurrentTemporaryRule = new Rule(_container, true);
                            _container.AddRule(_container.CurrentTemporaryRule);
                            _container.CurrentTemporaryRule.BeginActivity = this;
                            _container.CurrentTemporaryRule.BeginPointPosition = this.CenterPoint;
                            _container.CurrentTemporaryRule.EndPointPosition = _container.CurrentTemporaryRule.BeginPointPosition;
                            _container.CurrentTemporaryRule.ZIndex = _container.NextMaxIndex;
                            _container.CurrentTemporaryRule.RuleName = Text.NewRule;
                            _container.CurrentTemporaryRule.RuleCondition  = Text.RuleCondition;
                        }
                    }
                }
                else
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


            }
        }
        private void CenterEllipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDeleted)
                return;
            UserControl_MouseMove(sender, e);

        }

        private void CenterEllipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDeleted)
                return;
            if (_container.IsMouseSelecting || _container.CurrentTemporaryRule != null)
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
            UserControl_MouseLeftButtonUp(sender, e);
            if (_container.CurrentTemporaryRule != null)
            {
                if (_container.CurrentTemporaryRule.BeginActivity != null
                    && _container.CurrentTemporaryRule.BeginActivity == this)
                {
                    this.RemoveBeginRule(_container.CurrentTemporaryRule);
                    _container.RemoveRule(_container.CurrentTemporaryRule);
                    _container.CurrentTemporaryRule = null;

                }
            }

        }
    }
}
