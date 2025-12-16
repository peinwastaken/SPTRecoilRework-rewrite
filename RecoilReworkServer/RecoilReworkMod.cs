using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Utils;

namespace RecoilReworkServer
{
    [Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 2)]
    public class RecoilReworkMod(ISptLogger<RecoilReworkMod> logger) : IOnLoad
    {
        public Task OnLoad()
        {
            return Task.CompletedTask;
        }
    }
}
