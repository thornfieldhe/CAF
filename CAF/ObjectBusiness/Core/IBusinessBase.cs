namespace CAF
{
    using System;


    /// <summary>
    /// 确保是业务类
    /// </summary>
    public interface IEntityBase
    {
        Guid Id { get; }
        DateTime CreatedDate { get; }
        DateTime ChangedDate { get; }
        string Note { get; }
        int Status { get; set; }
        byte[] Version { get; }
    }
}