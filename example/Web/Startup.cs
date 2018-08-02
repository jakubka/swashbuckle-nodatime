using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
			else
				app.UseHsts();

			app
				.UseHttpsRedirection()
				.UseStaticFiles()
				.UseSwagger(o => o.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
				{
					swaggerDoc.Host = httpRequest.Host.Value;
					swaggerDoc.Schemes = new List<string> { httpRequest.Scheme };
				}))
				.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", Title))
				.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services
				.Configure<ApiBehaviorOptions>(o =>
				{
					o.InvalidModelStateResponseFactory = context =>
					{
						return new BadRequestObjectResult(
							new ValidationProblemDetails(context.ModelState)
							{
								Instance = context.HttpContext.Request.Path,
								Status = StatusCodes.Status400BadRequest,
								// Not sure what to put here just yet so send them to the swagger endpoint
								Type = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}/swagger",
								Detail = "Please refer to the errors property for additional details."
							})
						{
							ContentTypes = { "application/problem+json" }
						};
					};
				})
				.AddHsts(options =>
				{
					options.Preload = true;
					options.IncludeSubDomains = true;
					// Set TimeSpan to two years https://hstspreload.org/
					options.MaxAge = TimeSpan.FromDays(2 * 365);
				})
				.AddHttpsRedirection(o =>
				{
					o.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
					o.HttpsPort = 44370;
				})
				.AddSwaggerGen(o =>
				{
					o.SwaggerDoc("v1", new Info
					{
						Title = Title,
						Version = "v1"
					});
					o.OperationFilter<OperationFilter>();
					o.ConfigureForNodaTime(); //Note: you can pass JsonSerializerSettings in directly here I just leverage JsonConvert.DefaultSettings which is what gets called when no settings are passed
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
