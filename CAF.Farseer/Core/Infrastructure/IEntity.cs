namespace CAF.FS.Core.Infrastructure
{
    public interface IEntity<T>
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        T ID { get; set; }
    }

    public interface IEntity : IEntity<int?> { }
}
