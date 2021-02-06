using System;
using System.Linq;
using System.Linq.Expressions;

namespace LightLib.Service.Interfaces {
    /// <summary>
    /// Definition for a Paginator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPaginator<T> where T : class {
        
        public IQueryable<T> BuildPageResult<TOrder>(
            IQueryable<T> data, 
            int page, 
            int perPage, 
            Expression<Func<T, TOrder>> orderByExp, 
            bool isDescending = true
            );
        
        public IQueryable<T> BuildPageResult<TOrder>(
            IQueryable<T> data, 
            int page, 
            int perPage, 
            Expression<Func<T, bool>> whereExp,
            Expression<Func<T, TOrder>> orderByExp 
            );
    }
}