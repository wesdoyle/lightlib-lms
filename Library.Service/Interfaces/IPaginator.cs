using System;
using System.Linq;
using System.Linq.Expressions;

namespace Library.Service.Interfaces {
    public interface IPaginator<T> where T : class {
        
        public IQueryable<T> BuildPageResult<TOrder>(
            IQueryable<T> data, 
            int page, 
            int perPage, 
            Expression<Func<T, TOrder>> orderByExp);
        
        public IQueryable<T> BuildPageResult<TOrder>(
            IQueryable<T> data, 
            int page, 
            int perPage, 
            Expression<Func<T, bool>> whereExp,
            Expression<Func<T, TOrder>> orderByExp 
            );
    }
}