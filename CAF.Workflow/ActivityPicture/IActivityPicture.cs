using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
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


    public interface IActivityPicture
    {
        double Opacity
        {
            set;
            get;
        }
        double PictureWidth
        {
            get;
            set;
        }
        double PictureHeight
        {
            get;
            set;
        }
        Brush Background
        {
            set;
            get;
        }
        Visibility PictureVisibility
        {
            set;
            get;
        }
        void ResetInitColor();
        void SetWarningColor();
        void SetSelectedColor();
        PointCollection ThisPointCollection { get; }

    }
}
