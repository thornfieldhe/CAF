
namespace CAF
{
    using System;

    /// <summary>
    /// 确保是业务类
    /// </summary>
    public interface IEntityBase
    {
        Guid Id { get; set; }
    }
}
