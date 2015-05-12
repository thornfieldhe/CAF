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
namespace Shareidea.Web.UI.Control.Workflow.Designer
{

    public enum MergePictureRepeatDirection{ Vertical = 0, Horizontal,None } 
    public enum ActivityType { AND_BRANCH = 0, AND_MERGE, AUTOMATION, COMPLETION, DUMMY, INITIAL, INTERACTION, OR_BRANCH, OR_MERGE, SUBPROCESS, VOTE_MERGE }
    public enum WorkFlowElementType { Activity = 0, Rule,Label }
    public enum PageEditType { Add = 0, Modify ,None}
    public enum RuleLineType { Line = 0, Polyline }
    public enum HistoryType { New, Next, Previous };
    public class CheckResult
    {
        bool isPass=true;
        public bool IsPass { get { return isPass; } set { isPass = value; } }
      string message="";
      public string Message { get { return message; } set { message = value; } }
    }

    public interface IElement
    {

        CheckResult CheckSave();

        string ToXmlString();
        void LoadFromXmlString(string xmlString);
        void ShowMessage(string message);
        WorkFlowElementType ElementType { get; }

        PageEditType EditType { get; set; }

        bool IsSelectd { get; set; }
        IContainer Container { get; set; }
        void Delete();
        void UpperZIndex();
        bool IsDeleted { get; }
        void Zoom(double zoomDeep);

    }
}
