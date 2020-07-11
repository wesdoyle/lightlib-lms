using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Library.Service.Interfaces {
    public interface IPaginator<T> where T : class {
        
        public Task<List<T>> BuildPageResult<TOrder>(
            DbSet<T> data, 
            int page, 
            int perPage, 
            Expression<Func<T, TOrder>> orderByExp);
        
        public Task<List<T>> BuildPageResult<TOrder>(
            DbSet<T> data, 
            int page, 
            int perPage, 
            Expression<Func<T, bool>> whereExp,
            Expression<Func<T, TOrder>> orderByExp 
            );
    }
}