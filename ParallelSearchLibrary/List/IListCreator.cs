namespace ParallelSearchLibrary.List
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface with methods that help creating a word list.
    /// </summary>
    public interface IListCreator
    {
        /// <summary>
        /// Creates a new list with permutated words with the defined word length.
        /// </summary>
        /// <param name="wordLength">Number of characters in the words.</param>
        /// <returns>The list of <see cref="string"/> s.</returns>
        List<string> CreateWordList(int wordLength);
    }
}