using SPTarkov.Server.Core.Models.Spt.Mod;
using Range = SemanticVersioning.Range;
using Version = SemanticVersioning.Version;

namespace RecoilReworkServer
{
    public record Metadata : AbstractModMetadata
    {
        public override string ModGuid { get; init; } = "com.pein.recoilrework2";
        public override string Name { get; init; } = "Recoil Rework Rewrite";
        public override string Author { get; init; } = "pein";
        public override List<string>? Contributors { get; init; } = [];
        public override Version Version { get; init; } = new Version("1.0.0");
        public override Range SptVersion { get; init; } = new Range("~4.0.0");
        public override List<string>? Incompatibilities { get; init; } = [];
        public override Dictionary<string, Range>? ModDependencies { get; init; } = [];
        public override string? Url { get; init; } = "https://github.com/peinwastaken/SPTRecoilRework";
        public override bool? IsBundleMod { get; init; } = false;
        public override string License { get; init; } = "MIT";
    }
}
