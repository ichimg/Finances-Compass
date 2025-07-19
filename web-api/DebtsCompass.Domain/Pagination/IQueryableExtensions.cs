using Microsoft.EntityFrameworkCore;

namespace DebtsCompass.Domain.Pagination
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedResponse<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedResponse<T>(items, count, pageNumber, pageSize);
        }
    }
}
