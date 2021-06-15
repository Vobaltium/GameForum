using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GameForum_Washüttl.DomainModel.Interfaces
{
    public interface IReadOnlyService<TEntity>
        where TEntity : class, new()
    {
        IQueryable<TEntity> GetTable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    }
}