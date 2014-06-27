using CacheCow.Server;
using CacheCow.Server.EntityTagStore.Memcached;
using Enyim.Caching.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Tax.WebAPI.Caching
{
    public class CachingFactory
    {
        /// <summary>
        /// Gets the caching handler by cache store.
        /// </summary>
        /// <param name="cacheStore">The cache store.</param>
        /// <returns></returns>
        public static CachingHandler GetCachingHandlerByCacheStore(CachingStores cacheStore)
        {
            var cachingHandler = new CachingHandler();
            switch (cacheStore)
            {
                //case CachingStores.SqlCacheStore:
                //    cachingHandler = WithSqlCacheStore();
                //    break;
                case CachingStores.MemoryCacheStore:
                    cachingHandler = WithMemoryCacheStore();
                    break;
                case CachingStores.MemcachedCacheStore:
                    cachingHandler = WithMemcachedCacheStore();
                    break;
                //case CachingStores.MongoDbCacheStore:
                //    cachingHandler = WithMongoDbCacheStore();
                //    break;
                default:
                    break;
            }
            return cachingHandler;
        }

        /// <summary>
        /// With the memory cache store.
        /// </summary>
        /// <returns></returns>
        private static CachingHandler WithMemoryCacheStore()
        {
            var cacheHandler = new CachingHandler();
            return cacheHandler;
        }

        ///// <summary>
        ///// With the SQL cache store.
        ///// </summary>
        ///// <returns></returns>
        //private static CachingHandler WithSqlCacheStore()
        //{
        //    //You need to execute *.sql script included in the nuget package folder of CacheCow
        //    var connectionString = ConfigurationManager
        //        .ConnectionStrings["MySqlConnection"].ConnectionString;

        //    var eTagStore = new SqlServerEntityTagStore(connectionString);
        //    var cacheHandler = new CachingHandler(eTagStore);
        //    return cacheHandler;
        //}

        /// <summary>
        /// With the memcache cache store.
        /// </summary>
        /// <returns></returns>
        private static CachingHandler WithMemcachedCacheStore()
        {
            var eTagStore = new MemcachedEntityTagStore(ConfigurationManager
               .GetSection("enyim.com/memcached") as MemcachedClientSection);
            var cacheHandler = new CachingHandler(eTagStore);

            return cacheHandler;
        }

        ///// <summary>
        ///// With the mongo database cache store.
        ///// </summary>
        ///// <returns></returns>
        //private static CachingHandler WithMongoDbCacheStore()
        //{

        //    var connectionString = ConfigurationManager
        //        .ConnectionStrings["MyMongoDBConnection"].ConnectionString;

        //    var eTagStore = new MongoDbEntityTagStore(connectionString);
        //    var cacheHandler = new CachingHandler(eTagStore);

        //    return cacheHandler;
        //}
    }
}