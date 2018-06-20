using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.NodaTime.AspNetCore.Web.Filters;

namespace Swashbuckle.NodaTime.AspNetCore.Web
{
	public class Startup : IStartup
	{
		private const string Title = "Swashbuckle.NodaTime.AspNetCore Demo";
		private readonly IHostingEnvironment _env;

		public Startup(IHostingEnvironment hostingEnvironment)
		{
			_env = hostingEnvironment;
			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = { new StringEnumConverter() },
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.Indented
			}.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			if (_env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			app
				.UseStaticFiles()
				.UseSwagger()
				.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", Title))
				.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services
				.AddSwaggerGen(o =>
				{
					o.SwaggerDoc("v1", new Info
					{
						Title = Title,
						Version = "v1"
					});
					o.OperationFilter<OperationFilter>();
					o.ConfigureForNodaTime();
				})
				.AddMvcCore()
				.AddApiExplorer()
				.AddJsonFormatters(o =>
				{
					o.ContractResolver = new CamelCasePropertyNamesContractResolver();
					o.Converters.Add(new StringEnumConverter());
					o.NullValueHandling = NullValueHandling.Ignore;
					o.Formatting = Formatting.Indented;
					o.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
				});
			return services.BuildServiceProvider();
		}
	}
}
