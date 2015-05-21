
namespace CAF.Model
{
    using System.Data;

    public partial class Ccsq
    {
        public string ModuleName { get { return "出差申请"; } }

        public Workflow Workflow { get { return new Workflow(); } }

        public string[] Conditions
        {
            get
            {
                return new string[] { };
            }
        }



        protected override void PostInsert(IDbConnection conn, IDbTransaction transaction)
        {
            var user = User.Get(this.CreatedBy);
//            this.Workflow.CreateWorkflow(this.Id, this.ModuleName, user, conn, transaction, this.Conditions);
        }
    }
}
