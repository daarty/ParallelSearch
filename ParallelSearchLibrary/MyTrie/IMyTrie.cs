namespace ParallelSearchLibrary.MyTrie
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for my Trie implementations.
    /// </summary>
    public interface IMyTrie
    {
        /// <summary>
        /// Creates a new Trie with the passed list of words.
        /// </summary>
        /// <param name="wordList">List of words to be added to the trie.</param>
        void Create(List<string> wordList);

        /// <summary>
        /// Searches whithin the Trie for the given word.
        /// </summary>
        /// <param name="word">Word to search for.</param>
        /// <returns>List of all results.</returns>
        List<string> Search(string word);
    }
}