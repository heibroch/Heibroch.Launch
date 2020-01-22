using System.Collections.Generic;

namespace Heibroch.Launch.Interfaces
{
    public interface ICache<TKey, TTarget>
    {
        void Add(ICacheItem<TKey, TTarget> cacheItem);

        void Remove(ICacheItem<TKey, TTarget> cacheItem);
                
        void Add(ICacheIndex<TKey, TTarget> cacheIndex);

        void Remove(ICacheIndex<TKey, TTarget> cacheIndex);

        void Filter(ICacheFilter<TKey, TTarget> cacheFilter);

        IEnumerable<ICacheItem<TKey, TTarget>> QueryResults { get; }

        ICacheFilter<TKey, TTarget> CurrentCacheFilter { get; }
    }
}
