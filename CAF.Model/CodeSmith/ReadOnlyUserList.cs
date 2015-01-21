using System;

namespace CAF_Model
{
    using CAF;

    public class ReadOnlyUserList : ReadOnlyCollectionBase<ReadOnlyUser, ReadOnlyUserList>
    {
        public ReadOnlyUserList() : base("V_Users") { }
    }

    [Serializable]
    public class ReadOnlyUser
    {
        public Guid Id { get; internal set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime ChangedDate { get; internal set; }
        public string Note { get; internal set; }
        public string LoginName { get; internal set; }
        public string Name { get; internal set; }
        public string PhoneNum { get; internal set; }
        public Guid OrganizeId { get; internal set; }
        public string Email { get; internal set; }
        public string OrganizeName { get; internal set; }
        public Guid RoleId { get; internal set; }
    }
}


