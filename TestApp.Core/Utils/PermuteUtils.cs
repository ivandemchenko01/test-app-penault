namespace TestApp.Core.Utils
{
    public static class ListUtils
    {
        public static List<string> Combinations(this List<string> list, int combination = 2)
        {
            var result = new List<string>();
            // Nested loop for all possible pairs
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    string from = list[i];
                    string to = list[j];
                    if (from == to)
                        continue;

                    string pair = list[i].ToString() + list[j].ToString();

                    result.Add(pair);
                }
            }

            return result;
        }
    }
}
