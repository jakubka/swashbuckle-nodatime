using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Swashbuckle.NodaTime.AspNetCore.Web.Filters;

namespace Swashbuckle.NodaTime.AspNetCore.Web
{
	internal class Program
	{
		private const string _title = "Swashbuckle.NodaTime.AspNetCore Demo";

		private static Task Main(string[] args)
		{
			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = { new StringEnumConverter() },
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.Indented
			}.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

			return Host
				.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(web =>
				{
					web.ConfigureServices((hostContext, services) =>
					{
						services.AddSwaggerGen(o =>
						{
							o.SwaggerDoc("v1", new OpenApiInfo
							{
								Title = _title,
								Version = "v1"
							});
							o.OperationFilter<OperationFilter>();
							o.ConfigureForNodaTime(); //Note: you can pass JsonSerializerSettings in directly here I just leverage JsonConvert.DefaultSettings which is what gets called when no settings are passed
						})
						.AddControllers()
						.AddNewtonsoftJson(options =>
						{
							options.AllowInputFormatterExceptionMessages = true;
							options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
							options.SerializerSettings.Converters = new List<JsonConverter> { new StringEnumConverter() };
							options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
							options.SerializerSettings.Formatting = Formatting.Indented;
							options.SerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
						});
					}).Configure((hostContext, app) =>
					{
						app
							.UseStaticFiles()
							.UseSwagger(o => o.PreSerializeFilters.Add((apiDoc, httpReq) =>
							{
								apiDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
							}))
							.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", _title))
							.UseAuthentication()
							.UseRouting()
							.UseAuthorization()
							.UseEndpoints(endpoints =>
							{
								endpoints.MapControllers();
							});
					});
				})
				.Build()
				.RunAsync();
		}
	}
}
