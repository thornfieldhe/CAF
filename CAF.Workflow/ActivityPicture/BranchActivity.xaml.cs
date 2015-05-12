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
    public partial class BranchActivity : UserControl, IActivityPicture
    {
        public BranchActivity()
        {
            InitializeComponent();
        }
        public new double Opacity
        {
            set { picBRANCH.Opacity = value; }
            get { return picBRANCH.Opacity; }
        }
        double? _pictureWidth;
        public  double  PictureWidth
        { 
            get {
                if (_pictureWidth == null)
                {


                    _pictureWidth = Math.Abs(ThisPointCollection[1].X - ThisPointCollection[3].X);

                }
                return _pictureWidth.Value;
            }
            set
            {

                double width = value/2;
                double containerWidth = 100;
                double containerHeight = 80;
                PathGeometry pg = new PathGeometry();
                PathFigure pf = new PathFigure();
                pg.Figures.Add(pf);

                LineSegment ps = new LineSegment();
                ps.Point = new Point(containerWidth, containerHeight-width);
                pf.Segments.Add(ps);
                pf.StartPoint = ps.Point;


                ps = new LineSegment();
                ps.Point = new Point(containerWidth + width, containerHeight);
                pf.Segments.Add(ps);

                ps = new LineSegment();
                ps.Point = new Point(containerWidth , containerHeight+width);
                pf.Segments.Add(ps);

                ps = new LineSegment();
                ps.Point = new Point(containerWidth - width, containerHeight );
                pf.Segments.Add(ps);


                ps = new LineSegment();
                ps.Point = new Point(containerWidth, containerHeight - width);
                pf.Segments.Add(ps);
               
                picBRANCH.SetValue(Path.DataProperty, pg);
            }
        } 
        public  double PictureHeight
        { 
            get {

                return PictureWidth;
            }
            set
            {
                PictureWidth = value;
            }
        }

        PointCollection _thisPointCollection;
        public PointCollection ThisPointCollection
        {
            get
            {
                if (true)//(_thisPointCollection == null)
                {
                    _thisPointCollection = new PointCollection();


                    PathGeometry pg = (PathGeometry)picBRANCH.GetValue(Path.DataProperty); 

                    _thisPointCollection.Add(((LineSegment)pg.Figures[0].Segments[0]).Point);
                    _thisPointCollection.Add(((LineSegment)pg.Figures[0].Segments[1]).Point);
                    _thisPointCollection.Add(((LineSegment)pg.Figures[0].Segments[2]).Point);
                    _thisPointCollection.Add(((LineSegment)pg.Figures[0].Segments[3]).Point);
                }

                return _thisPointCollection;
            }
        }
        public new Brush Background
        {
            set { picBRANCH.Fill = value; 
            }
            get { return picBRANCH.Fill; }
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
            brush.Color = Colors.White;
            picBRANCH.Fill = brush;
            brush = new SolidColorBrush();
            brush.Color = Colors.Green;
            picBRANCH.Stroke = brush;
        }

        public void SetWarningColor()
        { 

            picBRANCH.Fill = SystemConst.ColorConst.WarningColor;   
        }
        public void SetSelectedColor()
        {
            picBRANCH.Fill = SystemConst.ColorConst.SelectedColor;   
        }
    }
}
