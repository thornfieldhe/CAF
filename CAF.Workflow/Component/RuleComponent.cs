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
    public class RuleComponent
    {

        public string LineType { get; set; }
        
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
            }
        }
    }
}
