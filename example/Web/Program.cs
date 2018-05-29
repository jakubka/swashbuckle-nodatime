using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Swashbuckle.NodaTime.AspNetCore.Web
{
	internal class Program
	{
		private static async Task Main(string[] args) => await WebHost
			.CreateDefaultBuilder(args)
			.UseSockets()
			.UseStartup<Startup>()
			.Build()
			.RunAsync();
	}
}
