using System.Collections.Generic;

namespace ParallelSearch.Utils
{
    /// <summary>
    /// Interface with methods that help creating a word list.
    /// </summary>
    public interface IListCreator
    {
        /// <summary>
        /// Creates a new list with permutated words.
        /// </summary>
        /// <returns>The list of <see cref="string"/>s.</returns>
        List<string> CreateWordList();
    }
}