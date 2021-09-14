using System.Collections.Immutable;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle.NodaTime.AspNetCore.Web.Filters;

internal sealed class OperationFilter : IOperationFilter
{
	private readonly IImmutableDictionary<int, string> _descriptionOverrides =
		new Dictionary<int, string>
		{
			[StatusCodes.Status201Created] = "Created",
			[StatusCodes.Status202Accepted] = "Accepted",
			[StatusCodes.Status204NoContent] = "No Content",
			[StatusCodes.Status415UnsupportedMediaType] = "Unsupported Media Type",
			[StatusCodes.Status422UnprocessableEntity] = "Unprocessable Entity",
			[StatusCodes.Status500InternalServerError] = "Internal Server Error",
			[StatusCodes.Status502BadGateway] = "Bad Gateway",
			[StatusCodes.Status503ServiceUnavailable] = "Service Unavailable",
			[StatusCodes.Status504GatewayTimeout] = "Gateway Timeout"
		}.ToImmutableDictionary();

	public void Apply(OpenApiOperation operation, OperationFilterContext context) =>
		operation.Responses?.ToList().ForEach(r =>
		{
			var keyVal = int.Parse(r.Key);
			if (_descriptionOverrides.ContainsKey(keyVal))
				r.Value.Description = _descriptionOverrides[keyVal];
		});
}
