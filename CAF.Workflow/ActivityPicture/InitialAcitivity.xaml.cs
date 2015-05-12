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
    public partial class InitialAcitivity : UserControl, IActivityPicture
    {
        public InitialAcitivity()
        {
            InitializeComponent();
        }
        public new double Opacity
        {
            set
            {
                picBegin.Opacity = value;
            }
            get { return picBegin.Opacity; }
        }
        public double PictureWidth
        {
            set
            {
                picBegin.Width = value - 4;
                eliBorder.Width = value - 2;
            }
            get { return picBegin.Width + 4; }
        }
        public double PictureHeight
        {
            set
            {
                picBegin.Height = value - 4;
                eliBorder.Height = value - 2;
            }
            get { return picBegin.Height + 4; }
        }
        public new Brush Background
        {
            set
            {
                picBegin.Fill = value;
            }
            get { return picBegin.Fill; }
        }
        public Visibility PictureVisibility
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
            brush.Color = Colors.Green;
            picBegin.Fill = brush;
            brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(255, 0, 0, 0);
        }

        public void SetWarningColor()
        {
            picBegin.Fill = SystemConst.ColorConst.WarningColor;
        }
        public void SetSelectedColor()
        {
            picBegin.Fill = SystemConst.ColorConst.SelectedColor;


        }
        public PointCollection ThisPointCollection
        {
            get { return null; }
        }
    }
}
