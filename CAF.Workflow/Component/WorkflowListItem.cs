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

namespace Shareidea.Web.UI.Control.Workflow.Designer.Component
{
    public class WorkflowListItem
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public WorkflowListItem()
        {
        }
        public WorkflowListItem(string name, string id)
        {
            Name = name;
            ID = id;
        }
    }
}
