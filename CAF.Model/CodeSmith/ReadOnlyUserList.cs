﻿using System;

namespace CAF.Model
{
    
    [Serializable]
    public partial class ReadOnlyUser :ITableName
    {        
    
        public ReadOnlyUser() { this.TableName = "V_Users"; }
		public Guid Id{get; set;}      
		public DateTime CreatedDate{get; set;}      
		public DateTime ChangedDate{get; set;}      
		public string Note{get; set;}      
		public string LoginName{get; set;}      
		public string Name{get; set;}      
		public string PhoneNum{get; set;}      
		public Guid OrganizeId{get; set;}      
		public string Email{get; set;}      
		public string OrganizeName{get; set;}      
		public string Level{get; set;}      
		public string Roles{get; set;}      
		public int Status{get; set;}      
		public string Abb{get; set;}      
        public string TableName { get; protected set; }
    }
}
