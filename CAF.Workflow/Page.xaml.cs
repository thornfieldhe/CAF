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
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using System.IO;
using Shareidea.Web.UI.Control.Workflow.Designer;
using Shareidea.Web.Component.Workflow;
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
namespace design
{

    public partial class Page : UserControl
    {

        public Page()
        {
            InitializeComponent();
           
            Container c = new Container();
            this.Content = c;
           
            
        }
    }
}
