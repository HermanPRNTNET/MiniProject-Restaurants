using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Common;

public class PagedResult<T>
{

    public PagedResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItemCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ItemsFrom = (pageNumber - 1) * pageSize + 1;
        ItemsTo = ItemsFrom + pageSize - 1;
    }

    public IEnumerable<T> Items { get; set; }
    public int TotalPages { get; set; }
    public int TotalItemCount { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
    //public List<object> Items { get; set; }
}
