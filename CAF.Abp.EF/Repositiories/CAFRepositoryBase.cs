using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Abp.EF.Repositiories
{
    using global::Abp.Domain.Entities;
    using global::Abp.EntityFramework;
    using global::Abp.EntityFramework.Repositories;

    public abstract class CAFRepositoryBase<TEnity,TPrimaryKey>:EfRepositoryBase<CAFDbContext,TEnity,TPrimaryKey>where TEnity:class,IEntity<TPrimaryKey>
   {
        protected CAFRepositoryBase(IDbContextProvider<CAFDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }


    public abstract class CAFRepositoryBase<TEntity> : CAFRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected CAFRepositoryBase(IDbContextProvider<CAFDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
