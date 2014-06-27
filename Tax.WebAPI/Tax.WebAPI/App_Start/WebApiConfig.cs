using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Web.Http.Cors;
using CacheCow.Server.EntityTagStore.Memcached;
using System.Configuration;
using CacheCow.Server;
using Tax.WebAPI.Caching;
using Enyim.Caching.Configuration;

namespace Tax.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //var cors = new EnableCorsAttribute("*", "*", "*") { SupportsCredentials = true };
            //config.EnableCors(cors);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            //var eTagStore = new MemcachedEntityTagStore(ConfigurationManager
            //                    .GetSection("enyim.com/memcached") as MemcachedClientSection);
            //var cacheHandler = new CachingHandler(eTagStore);
            //config.MessageHandlers.Add(cacheHandler);

            //CacheCow cache store
            config.MessageHandlers.Add(CachingFactory
               .GetCachingHandlerByCacheStore(CachingStores.MemoryCacheStore));
        }
    }
}
