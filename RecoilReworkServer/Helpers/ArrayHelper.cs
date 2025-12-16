namespace RecoilReworkServer.Helpers
{
    public static class ListHelper
    {
        private static Random random = new Random();

        public static T GetRandom<T>(this List<T> list)
        {
            return list[random.Next(0, list.Count)];
        }
    }
}
