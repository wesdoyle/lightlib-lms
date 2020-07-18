using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Library.Service.Interfaces {
    public interface IPaginator<T> where T : class {
        
        public IQueryable<T> BuildPageResult<TOrder>(
            DbSet<T> data, 
            int page, 
            int perPage, 
            Expression<Func<T, TOrder>> orderByExp);
        
        public IQueryable<T> BuildPageResult<TOrder>(
            DbSet<T> data, 
            int page, 
            int perPage, 
            Expression<Func<T, bool>> whereExp,
            Expression<Func<T, TOrder>> orderByExp 
            );

        public IIncludableQueryable<T, TInclude> BuildPageResult<TOrder, TInclude>(
            DbSet<T> data,
            int page,
            int perPage,
            Expression<Func<T, bool>> whereExp,
            Expression<Func<T, TOrder>> orderByExp,
            Expression<Func<T, TInclude>> includeExp
            );
    }
}