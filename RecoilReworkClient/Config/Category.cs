namespace RecoilReworkClient.Config
{
    public class Category
    {
        public static string Debug = "Debug";
        public static string General = "General";
        public static string RecoilParameters = "Base Recoil";
        public static string ShotBehavior = "Firing Behavior";
        public static string SprayPenalty = "Spray Penalty";
        public static string Stance = "Stance Multipliers";

        public static string Format(int order, string category) => $"{order:D2}. {category}";
    }
}
