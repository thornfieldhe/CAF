
namespace CAF.FSModels
{

    public partial class DirectoryRole : EFEntity<DirectoryRole>
    {
        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Role_Id:" + this.Role_Id);
            this.AddDescription("Directory_Id:" + this.Directory_Id);
        }
    }
}
