namespace ParallelSearchLibrary.Trie
{
    using System.Collections.Generic;
    using Result;
    using Timer;

    /// <summary>
    /// Interface of the Trie manager class.
    /// </summary>
    public interface ITrieManager
    {
        /// <summary>
        /// Gets or sets the currently selected Trie algorithm.
        /// </summary>
        TrieAlgorithm TrieAlgorithm { get; set; }

        /// <summary>
        /// Creates a new Trie from the given word list.
        /// </summary>
        /// <param name="wordList">List of words to be added to the trie.</param>
        /// <returns>The precise time span of the duration of the creation.</returns>
        PreciseTimeSpan CreateTrie(List<string> wordList);

        /// <summary>
        /// Searches for the given word whithin the current Trie .
        /// </summary>
        /// <param name="searchWord">Word to search for.</param>
        /// <param name="wordList">Current word list.</param>
        /// <returns>A search result object that contains the duration and the result list.</returns>
        SearchResult Search(string searchWord, List<string> wordList);
    }
}