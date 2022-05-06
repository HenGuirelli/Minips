using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minips2.Memories
{
    public class CacheCoherence
    {
        public HashSet<Cache> _caches = new();

        public void AddCache(Cache cache)
        {
            cache.AddCoeherence(this);
            _caches.Add(cache);
        }

        internal void InvalidCache(ICache cache, int address)
        {
            foreach (var cacheItem in _caches)
            {
                if (cacheItem != cache)
                {
                    cacheItem.InvalidCache(address);
                }
            }
        }

        internal bool TryGetCacheValue(ICache cache, int address, out byte[] resultFromAnotherCache)
        {
            foreach (var cacheItem in _caches)
            {
                if (cacheItem != cache)
                {
                    if (cacheItem.TryGetValueWithoutNextLevel(address, out var result))
                    {
                        resultFromAnotherCache = result;
                        return true;
                    }
                }
            }
            resultFromAnotherCache = null;
            return false;
        }
    }
}
