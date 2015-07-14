using System;
using System.Collections.Generic;

namespace CAF.Datas
{
    using CAF.Exceptions;
    using CAF.Utility;
    using System.Data.Entity;
    using System.Data.Entity.Core.Mapping;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Reflection;
    using DbContext = FS.Core.Data.DbContext;

    /// <summary>
    /// Entity Framework工作单元
    /// </summary>
    public abstract class EfUnitOfWork : DbContext, IUnitOfWork
    {
        /// <summary>
        /// 初始化Entity Framework工作单元
        /// </summary>
        /// <param name="connectionName">连接字符串的名称</param>
        protected EfUnitOfWork(string connectionName)
            : base(connectionName)
        {
            TraceId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 启动标识
        /// </summary>
        private bool IsStart { get; set; }

        /// <summary>
        /// 跟踪号
        /// </summary>
        public string TraceId { get; private set; }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            IsStart = true;
        }

        /// <summary>
        /// 提交更新
        /// </summary>
        public void Commit()
        {
            try
            {
                SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException(ex);
            }
            catch (DbEntityValidationException ex)
            {
                throw new EfValidationException(ex);
            }
            finally
            {
                IsStart = false;
            }
        }

        /// <summary>
        /// 通过启动标识执行提交，如果已启动，则不提交
        /// </summary>
        internal void CommitByStart()
        {
            if (IsStart)
                return;
            Commit();
        }

        /// <summary>
        /// 添加映射配置
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            foreach (IMap mapper in GetMaps())
                mapper.AddTo(modelBuilder.Configurations);
        }

        /// <summary>
        /// 获取映射配置
        /// </summary>
        private IEnumerable<IMap> GetMaps()
        {
            var result = new List<IMap>();
            foreach (var assembly in GetAssemblies())
                result.AddRange(Reflection.GetByInterface<IMap>(assembly));
            return result;
        }

        /// <summary>
        /// 获取定义映射配置的程序集列表
        /// </summary>
        protected virtual Assembly[] GetAssemblies()
        {
            return new[] { GetType().Assembly };
        }

        /// <summary>
        /// 初始化工作单元列表
        /// </summary>
        /// <param name="unitOfWorks">工作单元列表</param>
        public static void Init(params IUnitOfWork[] unitOfWorks)
        {
            foreach (var unitOfWork in unitOfWorks)
                InitUnitOfWork(unitOfWork);
        }

        /// <summary>
        /// 初始化工作单元
        /// </summary>
        private static void InitUnitOfWork(IUnitOfWork unitOfWork)
        {
            using (unitOfWork)
            {
                var objectContext = ((IObjectContextAdapter)unitOfWork).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
            }
        }
    }
}
