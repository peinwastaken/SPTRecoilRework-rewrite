using Newtonsoft.Json;
using RecoilReworkClient.Models;
using RootMotion.FinalIK;
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

        public static Dictionary<string, RecoilModifierData> FetchModifierData()
        {
            try
            {
                string route = "/recoilrework/weapondata";
                string response = RequestHandler.GetJson(route);
                return JsonConvert.DeserializeObject<Dictionary<string, RecoilModifierData>>(response);
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError(ex);
                throw;
            }
        }
    }
}
