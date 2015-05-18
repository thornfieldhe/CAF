using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Model
{
    using System.Data;

    using CAF.Data;

    public partial class WorkflowProcess
    {
       public static WorkflowProcess Get(string name)
       {
           using (IDbConnection conn = SqlService.Instance.Connection)
           {
               const string QUERY_GETBYNAME = "SELECT Top 1 * FROM Sys_WorkflowProcesses WHERE Name = @Name  AND Status!=-1";
               var item = conn.Query<WorkflowProcess>(QUERY_GETBYNAME, new { Name = name }).SingleOrDefault<WorkflowProcess>();
               if (item == null)
               {
                   return null;
               }
               item.Connection = SqlService.Instance.Connection;
               item.MarkOld();
               item._workflowActivityListInitalizer = new Lazy<WorkflowActivityList>(() => InitWorkflowActivitys(item), isThreadSafe: true);
               item._workflowRuleListInitalizer = new Lazy<WorkflowRuleList>(() => InitWorkflowRules(item), isThreadSafe: true);
               return item;
           }
       }
    }
}
