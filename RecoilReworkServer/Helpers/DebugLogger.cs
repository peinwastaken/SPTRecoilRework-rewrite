using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Utils.Logger;

namespace RecoilReworkServer.Helpers
{
    [Injectable(InjectionType.Singleton)]
    public class DebugLogger(SptLogger<DebugLogger> logger, ConfigHelper configHelper)
    {
        public void Log(string text, LogTextColor color, bool alwaysLog)
        {
            if (configHelper.EnableLogging || alwaysLog)
            {
                logger.LogWithColor($"[Recoil Rework] {text}", color);
            }
        }
        
        public void LogInfo(string text)
        {
            Log(text, LogTextColor.White, false);
        }
        
        public void LogWarning(string text)
        {
            Log(text, LogTextColor.Yellow, false);
        }

        public void LogError(string text)
        {
            Log(text, LogTextColor.Red, true);
        }

        public void LogSuccess(string text)
        {
            Log(text, LogTextColor.Green, false);
        }
    }
}
