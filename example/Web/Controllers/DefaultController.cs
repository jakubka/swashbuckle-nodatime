using Microsoft.AspNetCore.Mvc;

namespace Swashbuckle.NodaTime.AspNetCore.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true),
	Route("")]
public sealed class DefaultController : ControllerBase
{
	[HttpGet]
	public IActionResult Get() => Redirect("swagger");
}
