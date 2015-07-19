
namespace CAF.Models
{

    public partial class DirectoryRole : EFEntity<DirectoryRole>
    {
        #region 覆写基类方法

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Role_Id:" + this.Role_Id);
            this.AddDescription("Directory_Id:" + this.Directory_Id);
        }

        #endregion

    }
}
