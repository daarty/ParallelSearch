namespace ParallelSearch.Utils
{
    using System.Collections.Generic;

    /// <summary>
    /// Implementation of the <see cref="IListCreator"/> interface.
    /// </summary>
    public class ListCreator : IListCreator
    {
        private static readonly List<string> Characters = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        /// </inheritdoc>
        public List<string> CreateWordList()
        {
            var list = new List<string>();
            var indices = new List<int>();

            foreach (var first in Characters)
            {
                foreach (var second in Characters)
                {
                    foreach (var third in Characters)
                    {
                        foreach (var fourth in Characters)
                        {
                            indices.Add(list.Count);
                            list.Add($"{first}{second}{third}{fourth}");
                        }
                    }
                }
            }

            return list;
        }
    }
}