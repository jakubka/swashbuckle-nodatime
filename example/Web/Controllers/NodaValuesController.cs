using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.NodaTime.AspNetCore.Web.Models;

namespace Swashbuckle.NodaTime.AspNetCore.Web.Controllers;

[ApiController, Consumes(MediaTypeNames.Application.Json),
 Produces(MediaTypeNames.Application.Json), Route("api/[controller]")]
public sealed class NodaValuesController : ControllerBase
{
	[HttpGet]
	public ActionResult<IEnumerable<NodaValue>> Get() =>
		Ok(Enumerable.Range(0, 1).Select(_ => new NodaValue()));
}
