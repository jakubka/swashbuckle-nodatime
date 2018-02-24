using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.NodaTime.AspNetCore.Web.Models;

namespace Swashbuckle.NodaTime.AspNetCore.Web.Controllers
{
	[Consumes("application/json"),
	 Produces("application/json"),
	 Route("api/[controller]"),
	 ProducesResponseType(StatusCodes.Status415UnsupportedMediaType),
	 ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public class NodaValuesController : Controller
	{
		[HttpGet,
		 ProducesResponseType(typeof(IEnumerable<NodaValue>), StatusCodes.Status200OK)]
		public IActionResult Get() =>
			Ok(Enumerable.Range(0, 1).Select(i => new NodaValue()));
	}
}
