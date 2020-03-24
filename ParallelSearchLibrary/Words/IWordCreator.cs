namespace ParallelSearchLibrary.Words
{
    using ParallelSearchLibrary.Result;

    /// <summary>
    /// Interface with methods that help creating a word list.
    /// </summary>
    public interface IWordCreator
    {
        /// <summary>
        /// Creates a new list with permutated words with the defined word length.
        /// </summary>
        /// <param name="wordLength">Number of characters in the words.</param>
        /// <returns>The result containing the word list and the duration.</returns>
        WordListResult CreateWordList(int wordLength);

        /// <summary>
        /// Creates a new list with permutated words with the defined word length, using the
        /// multitasking ability of the system.
        /// </summary>
        /// <param name="wordLength">Number of characters in the words.</param>
        /// <returns>The result containing the word list and the duration.</returns>
        WordListResult CreateWordListParallel(int wordLength);

        /// <summary>
        /// Creates a random word with the given number of characters.
        /// </summary>
        /// <param name="numberOfCharacters">Maximum number of characters.</param>
        /// <returns>A random wird with the maximum number of characters.</returns>
        string GetRandomWord(int numberOfCharacters);
    }
}