using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.NodaTime.AspNetCore.Web.Models;

namespace Swashbuckle.NodaTime.AspNetCore.Web.Controllers
{
	[ApiController,
	 Consumes("application/json"),
	 Produces("application/json"),
	 Route("api/[controller]")]
	public class NodaValuesController : Controller
	{
		[HttpGet]
		public ActionResult<IEnumerable<NodaValue>> Get() =>
			Ok(Enumerable.Range(0, 1).Select(i => new NodaValue()));
	}
}
