using Microsoft.AspNetCore.Mvc;
using Swashbuckle.NodaTime.AspNetCore.Web.Models;

namespace Swashbuckle.NodaTime.AspNetCore.Web.Controllers;

[ApiController,
	Consumes("application/json"),
	Produces("application/json"),
	Route("api/[controller]")]
public sealed class NodaValuesController : ControllerBase
{
	[HttpGet]
	public ActionResult<IEnumerable<NodaValue>> Get() =>
		Ok(Enumerable.Range(0, 1).Select(i => new NodaValue()));
}
