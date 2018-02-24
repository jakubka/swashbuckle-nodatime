using Microsoft.AspNetCore.Mvc;

namespace Swashbuckle.NodaTime.AspNetCore.Web.Controllers
{
	[ApiExplorerSettings(IgnoreApi = true), Route("")]
    public class DefaultController : Controller
	{
		[HttpGet]
		public IActionResult Get() => Redirect("swagger");
	}
}
