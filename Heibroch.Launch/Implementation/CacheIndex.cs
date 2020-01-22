using Heibroch.Launch.Interfaces;
using System;
using System.Collections.Generic;

namespace Heibroch.Launch.Implementation
{
    public abstract class CacheIndex : ICacheIndex<string, string>
    {
        private Func<ICacheItem<string, string>, bool> indexPredicate;
        protected List<ICacheItem<string, string>> collection;

        public CacheIndex(Func<ICacheItem<string, string>, bool> indexPredicate) => this.indexPredicate = indexPredicate;

        public ICacheFilter<string, string> CurrentCacheFilter { get; protected set; }

        public IEnumerable<ICacheItem<string, string>> Collection => collection;

        public IEnumerable<ICacheItem<string, string>> QueryResults { get; protected set; }
        
        public abstract void Filter(ICacheFilter<string, string> cacheFilter);

        public abstract void Add(ICacheItem<string, string> cacheItem);

        public abstract void Remove(ICacheItem<string, string> cacheItem);
    }
}
