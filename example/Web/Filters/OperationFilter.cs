using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle.NodaTime.AspNetCore.Web.Filters
{
	internal class OperationFilter : IOperationFilter
	{
		private readonly IImmutableDictionary<int, string> _descriptionOverrides =
			new Dictionary<int, string>
			{
				{StatusCodes.Status202Accepted, "Accepted"},
				{StatusCodes.Status204NoContent, "No Content"},
				{StatusCodes.Status415UnsupportedMediaType, "Unsupported Media Type"},
				{StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity"},
				{StatusCodes.Status500InternalServerError, "Internal Server Error"}
			}.ToImmutableDictionary();

		public void Apply(Operation operation, OperationFilterContext context)
		{
			operation.Responses.ToList().ForEach(r =>
			{
				var keyVal = int.Parse(r.Key);
				if (_descriptionOverrides.ContainsKey(keyVal))
					r.Value.Description = _descriptionOverrides[keyVal];

			});
			operation.Parameters.Where(p => p.In == "body").ToList().ForEach(p => { p.Required = true; });
		}
	}
}
