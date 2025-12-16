using Newtonsoft.Json;
using RecoilReworkClient.Models;
using SPT.Common.Http;
using System;
using System.Collections.Generic;

namespace RecoilReworkClient.Helpers
{
    public static class RouteHelper
    {
        public static Dictionary<string, CaliberData> FetchCaliberData()
        {
            try
            {
                string route = "/recoilrework/caliberdata";
                string response = RequestHandler.GetJson(route);
                return JsonConvert.DeserializeObject<Dictionary<string, CaliberData>>(response);
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError(ex);
                throw;
            }
        }
    }
}
