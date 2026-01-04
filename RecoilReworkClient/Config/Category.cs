namespace RecoilReworkClient.Config
{
    public class Category
    {
        public static string General = "General";
        public static string WeaponKick = "Weapon Kick";
        public static string Stance = "Stance";

        public static string Format(int order, string category) => $"{order.ToString("D2")}. {category}";
    }
}
