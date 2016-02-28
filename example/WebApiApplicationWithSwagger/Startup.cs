using System.Web.Http;

using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;

using NodaTime;
using NodaTime.Serialization.JsonNet;
using Swashbuckle.Application;
using Swashbuckle.NodaTime;
using WebApiApplicationWithSwagger;

[assembly: OwinStartup(typeof (Startup))]

namespace WebApiApplicationWithSwagger
{
    public class Startup
    {
        public void Configuration(
            IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Formatters.Remove(httpConfiguration.Formatters.XmlFormatter);

            var jsonSerializerSettings = httpConfiguration.Formatters.JsonFormatter.SerializerSettings;
            jsonSerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

            ConfigureSwagger(httpConfiguration, jsonSerializerSettings);

            app.UseWebApi(httpConfiguration);
        }

        private static void ConfigureSwagger(
            HttpConfiguration httpConfiguration,
            JsonSerializerSettings jsonSerializerSettings)
        {
            httpConfiguration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Swashbuckle.NodaTime example");

                    // INTERESTING LINE:
                    c.ConfigureForNodaTime(jsonSerializerSettings);
                })
                .EnableSwaggerUi();
        }
    }
}