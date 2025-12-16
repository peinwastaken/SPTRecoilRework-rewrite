using RecoilReworkServer.Helpers;
using RecoilReworkServer.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Utils;
using SPTarkov.Server.Core.Utils.Logger;

namespace RecoilReworkServer.Routes
{
    [Injectable]
    public class RecoilReworkRouter : StaticRouter
    {
        private static HttpResponseUtil? _httpResponseUtil;
        private static JsonUtil? _jsonUtil;

        public RecoilReworkRouter(
            JsonUtil jsonUtil,
            HttpResponseUtil httpResponseUtil,
            SptLogger<RecoilReworkRouter> logger) : base(
            jsonUtil,
            GetRoutes()
        )
        {
            _httpResponseUtil = httpResponseUtil;
            _jsonUtil = jsonUtil;
        }

        private static List<RouteAction> GetRoutes()
        {
            return
            [
                new RouteAction(
                    "/recoilrework/caliberdata",
                    async (url, info, sessionId, output) =>
                    {
                        Dictionary<string, CaliberData> data = Globals.CaliberData;
                        string json = _jsonUtil?.Serialize(data) ?? "{}";
                        return json;
                    }
                )
            ];
        }
    }
}
