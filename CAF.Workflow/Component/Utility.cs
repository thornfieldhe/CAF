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
using Shareidea.Web.UI.Control.Workflow.Designer;
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
namespace Shareidea.Web.Component.Workflow
{
    public class Configure
    {
        static System.Globalization.CultureInfo currentCulture;
        public static System.Globalization.CultureInfo CurrentCulture
        {
            get
            {

                if (currentCulture == null)
                {
                    try
                    {
                        System.IO.IsolatedStorage.IsolatedStorageSettings appSetting = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
                        if (appSetting.Contains("language"))
                        {
                            currentCulture = new System.Globalization.CultureInfo((string)appSetting["language"]);
                        }
                    }
                    catch (Exception e)
                    {
                    }
                   
                } 
                if (currentCulture == null)
                {
                    currentCulture =new System.Globalization.CultureInfo("en-us");

                }
                return currentCulture;
            }
            set
            {
                currentCulture = value;
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = currentCulture;

                try
                {
                    System.IO.IsolatedStorage.IsolatedStorageSettings appSetting = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
                    if (appSetting.Contains("language"))
                    {
                        appSetting["language"] = currentCulture.Name;
                    }
                    else
                    {
                        appSetting.Add("language", currentCulture.Name);
                    }

                }
                catch (Exception e)
                {
                }
            }
        }
    }
    public class Utility
    {
        public static void SetOnMouseEnter(UIElement element)
        {
        }
    }
}