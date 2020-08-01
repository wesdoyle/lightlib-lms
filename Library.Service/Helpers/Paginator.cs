using System;
using System.Linq;
using System.Linq.Expressions;
using Library.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Service.Helpers {
    public class Paginator<T> : IPaginator<T> where T: DbSet<T> {
        /// <summary>
        /// Builds a paginated list of EF DbSet data 
        /// </summary>
        /// <param name="dbSet">EF Core DbSet</param>
        /// <param name="page">The Page number to query</param>
        /// <param name="perPage">The number of entities per page</param>
        /// <param name="orderByExp">Order By lambda</param>
        /// <param name="isDescending"></param>
        /// <typeparam name="TOrder">Entity Property to order by</typeparam>
        /// <returns></returns>
        public IQueryable<T> BuildPageResult<TOrder>(
            IQueryable<T> dbSet, 
            int page, 
            int perPage, 
            Expression<Func<T, TOrder>> orderByExp, bool isDescending=true) {
            var entsToSkip = (page - 1) * perPage;

            if (isDescending) {
                return dbSet
                    .OrderByDescending(orderByExp)
                    .Skip(entsToSkip)
                    .Take(perPage);
            }

            return dbSet
                .OrderBy(orderByExp)
                .Skip(entsToSkip)
                .Take(perPage);
        }

        /// <summary>
        /// Builds a paginated list of EF DbSet data with a LINQ where
        /// expression and LINQ OrderBy expression
        /// </summary>
        /// <param name="dbSet">EF Core DbSet</param>
        /// <param name="page">The Page number to query</param>
        /// <param name="perPage">The number of entities per page</param>
        /// <param name="whereExp">a LINQ Where expression</param>
        /// <param name="orderExp">a LINQ OrderBy expression</param>
        /// <typeparam name="TOrder">Entity Property to order by</typeparam>
        /// <returns></returns>
        public IQueryable<T> BuildPageResult<TOrder>(
            IQueryable<T> dbSet, int page, int perPage, 
            Expression<Func<T, bool>> whereExp, 
            Expression<Func<T, TOrder>> orderExp) {
            var entsToSkip = (page - 1) * perPage;

            return dbSet
                .OrderBy(orderExp)
                .Where(whereExp)
                .Skip(entsToSkip)
                .Take(perPage);
        }
    }
}