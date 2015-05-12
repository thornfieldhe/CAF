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

namespace Shareidea.Web.UI.Control.Workflow.Designer
{
    public partial class Label : UserControl, IElement
    {
        public CheckResult CheckSave()
        {
            CheckResult cr = new CheckResult();
            cr.IsPass = true;
            return cr;
        }
        public string ToXmlString()
        {
            if(!isDeleted)
                return @"<Label X=""" + Position.X.ToString() + @""" Y=""" + Position.Y.ToString() + @"""><![CDATA[" + LabelName + "]]></Label>";
            return "";
        }
        public void LoadFromXmlString(string xml)
        {
        }
        public void ShowMessage(string msg)
        {
        }
        public void SetPositionByDisplacement(double x, double y)
        {


            Point p = new Point();
            p.X = (double)this.GetValue(Canvas.LeftProperty);
            p.Y = (double)this.GetValue(Canvas.TopProperty);

            this.SetValue(Canvas.TopProperty, p.Y + y);
            this.SetValue(Canvas.LeftProperty, p.X + x);
            

        }
       public  Label Clone()
        {
            Label l = new Label(_container);
            l.LabelName = this.LabelName;
            l.Position = this.Position;
            return l;
        } 
        public WorkFlowElementType ElementType
        {
            get
            {
                return WorkFlowElementType.Label;
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
        public Label()
        {
        }
       
        public Label(IContainer container)
        {
            _container = container;
            _doubleClickTimer = new System.Windows.Threading.DispatcherTimer();
            _doubleClickTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            _doubleClickTimer.Tick += new EventHandler(DoubleClick_Timer);
            System.Windows.Browser.HtmlPage.Document.AttachEvent("oncontextmenu", OnContextMenu);

            InitializeComponent();
        }
        private void OnContextMenu(object sender, System.Windows.Browser.HtmlEventArgs e)
        {

            if (_container.MouseIsInContainer)
            {
                e.PreventDefault();

                if (canShowMenu && !IsDeleted)
                {

                    _container.ShowLabelContentMenu(this, sender, e);
                }
            }
        }
        void DoubleClick_Timer(object sender, EventArgs e)
        {
            _doubleClickTimer.Stop();
        }
        public string LabelName
        {
            get
            {
                return txtLabelName.Text;
            }
            set
            {
                tbLabelName.Text = value;
                txtLabelName.Text = value;
            }
        }
        bool canShowMenu = false;
        bool isSelectd = false;
        void SetSelectedColor()
        { 

            txtLabelName.Foreground = SystemConst.ColorConst.SelectedColor;
        }
        void ResetInitColor()
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Black;
            txtLabelName.Foreground = brush;
        }
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
                    SetSelectedColor();

                    if (!_container.CurrentSelectedControlCollection.Contains(this))
                        _container.AddSelectedControl(this);



                }
                else
                {
                    ResetInitColor();
                }
            }

        }
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
                this.Visibility = Visibility.Collapsed;
                _container.RemoveLabel(this);
                 
            }
        }

        public void Zoom(double zoomDeep)
        {
            

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
        public void UpperZIndex()
        {
            ZIndex = _container.NextMaxIndex;
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
               if(hadActualMove)
                   _container.SaveChange(HistoryType.New);
            FrameworkElement element = sender as FrameworkElement;
            trackingMouseMove = false;
            element.ReleaseMouseCapture();

            mousePosition.X = mousePosition.Y = 0;
            element.Cursor = null;
                
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
        bool hadActualMove = false;
        Point mousePosition;
        bool trackingMouseMove = false;
        System.Windows.Threading.DispatcherTimer _doubleClickTimer;

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            hadActualMove = false;
           


             e.Handled = true;
            hadActualMove = false;
            if (_doubleClickTimer.IsEnabled)
            {
                _doubleClickTimer.Stop();
                tbLabelName.Visibility = Visibility.Visible;
                txtLabelName.Visibility = Visibility.Collapsed;

            }
            else
            {
                _doubleClickTimer.Start();
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
        public Point Position
        {
            get
            {
                Point position;

                position = new Point();
                position.Y = (double)this.GetValue(Canvas.TopProperty);
                position.X = (double)this.GetValue(Canvas.LeftProperty) ;  
                return position;
            }
            set
            {

                this.SetValue(Canvas.TopProperty, value.Y);
                this.SetValue(Canvas.LeftProperty, value.X);
                
            }
        }
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
                if (false
                    )
                {
                    //超过流程容器的范围
                }
                else
                {
                    this.SetValue(Canvas.TopProperty, newTop);
                    this.SetValue(Canvas.LeftProperty, newLeft);

                    //Move(this, e);
                    mousePosition = e.GetPosition(null);
                    _container.MoveControlCollectionByDisplacement(deltaH, deltaV, this);

                }


            }
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            txtLabelName.Visibility = Visibility.Visible;
            tbLabelName.Visibility = Visibility.Collapsed;
            canShowMenu = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtLabelName.Text = tbLabelName.Text;
        }
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            canShowMenu = true;

               return;

        } 
    }
}
