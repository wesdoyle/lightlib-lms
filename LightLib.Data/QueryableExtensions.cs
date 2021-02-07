using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LightLib.Models;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Data {
    public static class QueryableExtensions {
        public static async Task<PaginationResult<T>> ToPaginatedResult<T> (
            this IQueryable<T> query, int page, int perPage) {
            
            var count = await query.CountAsync();

            var filtered = await query
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToListAsync();

            return new PaginationResult<T> {
                TotalCount = count,
                Results = filtered,
                PerPage = perPage,
                PageNumber = page
            };
        }


        public static IQueryable<T> OrderByAtRuntime<T>(
            this IQueryable<T> source, 
            string columnName, 
            bool isDescending = true) {
            if (string.IsNullOrEmpty(columnName)) {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, "");
            MemberExpression property = Expression.Property(parameter, columnName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = isDescending ? "OrderByDescending" : "OrderBy";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                new Type[] { source.ElementType, property.Type },
                source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }}