using Microsoft.EntityFrameworkCore;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Dal.Extensions
{
    public static class PagedQuery
    {
        public static async Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query,
            int page, int pageSize, bool sortOrder = false) where T : Sortable
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;

            query = sortOrder ? query.OrderByDescending(x => x.Date) : query.OrderBy(x => x.Date);

            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
    }

}
