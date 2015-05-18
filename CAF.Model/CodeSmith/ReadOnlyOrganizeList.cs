using System;

namespace CAF.Model
{
    
    [Serializable]
    public partial class ReadOnlyOrganize :ITableName
    {        
    
        public ReadOnlyOrganize() { this.TableName = "V_Organizes"; }
		public DateTime ChangedDate{get; set;}      
		public string Code{get; set;}      
		public DateTime CreatedDate{get; set;}      
		public Guid Id{get; set;}      
		public string Level{get; set;}      
		public string Name{get; set;}      
		public Guid ParentId{get; set;}      
		public int Sort{get; set;}      
		public string ParentName{get; set;}      
		public int SysLevel{get; set;}      
		public int Status{get; set;}      
        public string TableName { get; protected set; }
    }
}
