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
        public static async Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query
                                                            ,int page, int pageSize) where T : Sortable
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
            result.Results = await query.OrderBy(x => x.Date).Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
    }

}
