using System.Collections.Generic;

namespace ParallelSearch.Utils
{
    /// <summary>
    /// Implementation of the <see cref="IListCreator"/> interface.
    /// </summary>
    public class ListCreator : IListCreator
    {
        /// </inheritdoc>
        public List<string> CreateWordList()
        {
            var list = new List<string>();

            // TODO implement the creation
            list.Add("A");
            list.Add("B");
            list.Add("C");

            return list;
        }
    }
}