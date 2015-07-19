using System;

namespace CAF.Models
{
    public class DirectoryRoleListView
    {
        public Guid Id { get; set; }
        public Guid DirectoryId { get; set; }
        public Guid RoleId { get; set; }
        public int Status { get; set; }
        public string DirectoryName { get; set; }
        public string RoleName { get; set; }
        public string TableName { get; protected set; }
        public string StatusName
        {
            get
            {
                return CAF.Utility.Enum.GetDescription<RightStatusEnum>(this.Status);
            }
        }
    }
}
