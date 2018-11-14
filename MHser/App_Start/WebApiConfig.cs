using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MHser
{
    public static class WebApiConfig
    {
        public static void Register( HttpConfiguration config )
        {
            config.Formatters.Remove( config.Formatters.XmlFormatter );
            // Конфигурация и службы веб-API
            EnableCorsAttribute corsAttribute =
                new EnableCorsAttribute( "http://localhost:8080", "*", "*" )
                {
                    SupportsCredentials = true
                };
            // Маршруты веб-API
            config.MapHttpAttributeRoutes();
            config.EnableCors( corsAttribute );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
