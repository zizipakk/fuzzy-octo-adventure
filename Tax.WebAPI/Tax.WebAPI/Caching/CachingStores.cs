using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax.WebAPI.Caching
{
    public enum CachingStores
    {
        ///// <summary>
        ///// The SQL cache store
        ///// </summary>
        //SqlCacheStore = 0,
        /// <summary>
        /// The memory cache store
        /// </summary>
        MemoryCacheStore = 1,
        /// <summary>
        /// The memcached cache store
        /// </summary>
        MemcachedCacheStore = 2,
        ///// <summary>
        ///// The mongo database cache store
        ///// </summary>
        //MongoDbCacheStore = 3
    }
}