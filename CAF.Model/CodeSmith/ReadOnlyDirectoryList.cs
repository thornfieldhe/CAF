﻿using System;

namespace CAF.Model
{
    
    [Serializable]
    public partial class ReadOnlyDirectory :ITableName
    {        
    
        public ReadOnlyDirectory() { this.TableName = "V_Directories"; }
		public Guid Id{get; set;}      
		public string Name{get; set;}      
		public string Url{get; set;}      
		public Guid ParentId{get; set;}      
		public int Sort{get; set;}      
		public string Note{get; set;}      
		public int Status{get; set;}      
		public string Level{get; set;}      
		public int x{get; set;}      
		public int y{get; set;}      
		public DateTime CreatedDate{get; set;}      
		public DateTime ChangedDate{get; set;}      
		public string ParentName{get; set;}      
		public int SysLevel{get; set;}      
        public string TableName { get; protected set; }
    }
}
