namespace ParallelSearchLibrary.Trie
{
    /// <summary>
    /// Specifies different types of Trie algorithms.
    /// </summary>
    public enum TrieAlgorithm
    {
        /// <summary>
        /// My Trie implementation.
        /// </summary>
        MyTrie,

        /// <summary>
        /// My parallelized Trie implementation.
        /// </summary>
        MyParallelTrie,

        /// <summary>
        /// Basic Trie implementation from Trie.Net.
        /// </summary>
        Basic,

        /// <summary>
        /// Concurrent Trie implementation from Trie.Net.
        /// </summary>
        Concurrent,

        /// <summary>
        /// Patricia Trie implementation from Trie.Net.
        /// </summary>
        Patricia,

        /// <summary>
        /// Ukkonen Trie implementation from Trie.Net.
        /// </summary>
        Ukkonen
    }
}