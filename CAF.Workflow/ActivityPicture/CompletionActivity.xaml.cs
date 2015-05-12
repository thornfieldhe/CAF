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
namespace Shareidea.Web.UI.Control.Workflow.Designer.Picture
{
    public partial class CompletionActivity : UserControl, IActivityPicture
    {
        public CompletionActivity()
        {
            InitializeComponent();
        }
        public new double Opacity
        {
            set { picEnd.Opacity = value; }
            get { return picEnd.Opacity; }
        }
        public  double PictureWidth
        {
            set { picEnd.Width = value-4;
            eliBorder.Width = value - 2;
            
            }
            get { return picEnd.Width+4; 
                    
            }
        }
        public  double PictureHeight
        {
            set { picEnd.Height = value-4;
            eliBorder.Height = value - 2;
            }
            get { return picEnd.Height+4; }
        }
        public new   Brush Background
        {
            set { picEnd.Fill = value;
            //picCenter.Fill = value;
            }
            get { return picEnd.Fill; }
        }
        public  Visibility PictureVisibility
        {
            set
            {

                this.Visibility = value;
            }
            get
            {

                return this.Visibility;
            }
        }
        public void ResetInitColor()
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Red;
            picEnd.Fill = brush;
            brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(255, 0, 0, 0); 
           // picCenter.Fill = brush;
        }

        public void SetWarningColor()
        { 
            picEnd.Fill = SystemConst.ColorConst.WarningColor; 
          //  picCenter.Fill = SystemConst.ColorConst.WarningColor;  
        }
        public void SetSelectedColor()
        {

            picEnd.Fill = SystemConst.ColorConst.SelectedColor;
          //  picCenter.Fill = SystemConst.ColorConst.SelectedColor;  

        }
        public PointCollection ThisPointCollection
        {
            get { return null; }
        }
    }
}
