using Heibroch.Launch.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Heibroch.Launch.Implementation
{
    public class Cache : ICache<string, string>
    {
        public List<ICacheIndex<string, string>> cacheIndeces = new List<ICacheIndex<string, string>>();

        public IEnumerable<ICacheItem<string, string>> QueryResults => cacheIndeces.SelectMany(x => x.QueryResults);

        public ICacheFilter<string, string> CurrentCacheFilter => cacheIndeces.FirstOrDefault()?.CurrentCacheFilter;

        public void Add(ICacheItem<string, string> cacheItem) => cacheIndeces.ForEach(x => x.Add(cacheItem));

        public void Remove(ICacheItem<string, string> cacheItem) => cacheIndeces.ForEach(x => x.Remove(cacheItem));

        public void Add(ICacheIndex<string, string> cacheIndex)
        {
            if (cacheIndeces.Contains(cacheIndex)) return;
            cacheIndeces.Add(cacheIndex);
        }

        public void Remove(ICacheIndex<string, string> cacheIndex)
        {
            if (!cacheIndeces.Contains(cacheIndex)) return;
            cacheIndeces.Add(cacheIndex);
        }

        public void Filter(ICacheFilter<string, string> cacheFilter) => cacheIndeces.ForEach(x => x.Filter(cacheFilter));
    }
}
