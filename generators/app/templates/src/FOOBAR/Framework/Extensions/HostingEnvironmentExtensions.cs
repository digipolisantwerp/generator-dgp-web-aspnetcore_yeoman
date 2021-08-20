using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FOOBAR
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsVagrant(this IWebHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment("vagrant");
        }
    }
}
