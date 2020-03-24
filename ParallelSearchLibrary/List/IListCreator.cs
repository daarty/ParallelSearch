namespace ParallelSearchLibrary.List
{
    using ParallelSearchLibrary.Result;

    /// <summary>
    /// Interface with methods that help creating a word list.
    /// </summary>
    public interface IListCreator
    {
        /// <summary>
        /// Creates a new list with permutated words with the defined word length.
        /// </summary>
        /// <param name="wordLength">Number of characters in the words.</param>
        /// <returns>The result containing the word list and the duration.</returns>
        WordListResult CreateWordList(int wordLength);
    }
}