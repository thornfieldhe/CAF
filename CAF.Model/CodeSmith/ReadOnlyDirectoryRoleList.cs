using System;

namespace CAF.Model
{
    
    [Serializable]
    public partial class ReadOnlyDirectoryRole :ITableName
    {        
    
        public ReadOnlyDirectoryRole() { this.TableName = "V_DirectoryRoles"; }
		public Guid Id{get; set;}      
		public Guid DirectoryId{get; set;}      
		public Guid RoleId{get; set;}      
		public int Status{get; set;}      
		public string DirectoryName{get; set;}      
		public string RoleName{get; set;}      
        public string TableName { get; protected set; }
    }
}
