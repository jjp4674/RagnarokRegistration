using DotNetNuke.Web.Api;

namespace Ragnarok.Modules.RagnarokRegistration.Services
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("RagnarokRegistration", "default", "{controller}/{action}", new[]
            {
                "Ragnarok.Modules.RagnarokRegistration.Services"
            });
        }
    }
}