using Demo.Service.Models;
using Microsoft.OData.Edm;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace Demo.Service {
    // Packages to include
    // Install-Package Microsoft.AspNet.WebApi.Cors
    // Install-Package Microsoft.AspNet.OData 

    public static class WebApiConfig {

        public static void Register(HttpConfiguration config) {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Enable queries
            config.Select().Expand().Filter().OrderBy().MaxTop(null).Count();

            // Enables support for CORS
            var corsAttr = new EnableCorsAttribute(origins: "*", headers: "*", methods: "*");
            corsAttr.SupportsCredentials = true;
            config.EnableCors(corsAttr);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.MapODataServiceRoute("odata", "odata", GetEdmModel());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static IEdmModel GetEdmModel() {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "Demos";
            builder.ContainerName = "DefaultContainer";

            builder.EntityType<Message>();

            var incidentType = builder.EntityType<Incident>();
            incidentType.Ignore(p => p.Status);
            incidentType.Property(p => p.NewMessage);
            incidentType.Property(p => p.StatusString);
            incidentType.Property(p => p.StatusColor);
            incidentType.Property(p => p.ContactFullName);
            incidentType.Property(p => p.History);
            incidentType.Property(p => p.ContactOid).IsOptional();
            builder.EntitySet<Incident>("Incidents");

            builder.EntitySet<Person>("Persons");

            builder.Namespace = typeof(Incident).Namespace;

            return builder.GetEdmModel();
        }

    }
}
