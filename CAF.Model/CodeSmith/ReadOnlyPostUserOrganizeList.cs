using System;

namespace CAF.Model
{
    
    [Serializable]
    public partial class ReadOnlyPostUserOrganize :ITableName
    {        
    
        public ReadOnlyPostUserOrganize() { this.TableName = "V_PostUserOrganizes"; }
		public Guid Id{get; set;}      
		public Guid OrganizeId{get; set;}      
		public Guid PostId{get; set;}      
		public Guid UserId{get; set;}      
		public string UserName{get; set;}      
		public string PostName{get; set;}      
		public string OrganizeName{get; set;}      
        public string TableName { get; protected set; }
    }
}
