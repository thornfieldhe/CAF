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
namespace Shareidea.Web.Component.Workflow 
{
    public class ActivityComponent
    {

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

        string _repeatDirection = "Horizontal";
        public string RepeatDirection
        {
            get
            {
                return _repeatDirection;
            }
            set
            {
                _repeatDirection = value;
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

         string activityName;

        public string ActivityName
        {
            get
            {
                return activityName;
            }
            set
            {
                activityName = value;
            }
        } 
         string activityType; 
        public string ActivityType
        {
            get
            {
                return activityType;
            }
            set
            {
                activityType = value;
            }
        }

        string activityPost;
        public string ActivityPost
        {
            get
            {
                return activityPost;
            }
            set
            {
                activityPost = value;
            }
        }
    }
}
