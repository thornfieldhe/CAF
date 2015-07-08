using System;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using System.Data;


    public partial class WfProcess
    {
        public static WfProcessList GetAllByModuleId(Guid moduleId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                const string QUERY_GETAllByModuleId = "SELECT * FROM Sys_WfProcesses WHERE  Status!=-1 And ModulelId=@ModulelId";
                var items = conn.Query<WfProcess>(QUERY_GETAll, new { ModulelId = moduleId }).ToList();

                var list = new WfProcessList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkClean();
                    item._wfActivityListInitalizer = new Lazy<WfActivityList>(() => InitWfActivitys(item), isThreadSafe: true);
                    item._wfRuleListInitalizer = new Lazy<WfRuleList>(() => InitWfRules(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkClean();
                return list;
            }
        }

        public IBusinessBase Module
        {
            get
            {
                switch (this.Name)
                {
                    case "出差申请":
                        return Ccsq.Get(this.ModulelId);
                    default:
                        return null;
                }
            }
        }
    }
}
