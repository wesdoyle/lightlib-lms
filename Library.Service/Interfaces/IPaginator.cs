using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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
    }
}