using System;
using System.Collections.Generic;
using System.Linq;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Extensions
{
    public static class CollectionExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> collection, int pageNumber, int pageSize)
            where T : class
            => PagedList<T>.Create(collection, pageNumber, pageSize);

        public static IEnumerable<T> SortRandom<T>(this IEnumerable<T> collection)
            => collection.OrderBy(x => Guid.NewGuid());
    }
}