using Heibroch.Launch.Interfaces;
using System.Collections.Generic;

namespace Heibroch.Launch.Interfaces
{
    public interface ICacheIndex<TKey, TTarget>
    {
        void Add(ICacheItem<TKey, TTarget> cacheItem);

        void Remove(ICacheItem<TKey, TTarget> cacheItem);

        void Filter(ICacheFilter<TKey, TTarget> cacheFilter);
        
        IEnumerable<ICacheItem<TKey, TTarget>> QueryResults { get; }

        ICacheFilter<TKey, TTarget> CurrentCacheFilter { get; }

        IEnumerable<ICacheItem<TKey, TTarget>> Collection { get; }
    }
}
