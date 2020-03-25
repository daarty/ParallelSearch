namespace ParallelSearchLibrary.MyTrie
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Implements the <see cref="IMyTrie"/> interface so it can be run parallelized.
    /// </summary>
    public class MyParallelTrie : IMyTrie
    {
        /// <summary>
        /// Creates a new instance of <see cref="MyParallelTrie"/>.
        /// </summary>
        /// <param name="wordList">List of words to be added to the trie.</param>
        public MyParallelTrie(List<string> wordList)
        {
            this.Create(wordList);
        }

        /// <summary>
        /// Gets the root node of the Trie-tree.
        /// </summary>
        public ParallelNode RootNode { get; } = new ParallelNode();

        /// </inheritdoc>
        public void Create(List<string> wordList)
        {
            Parallel.ForEach(wordList, word => RootNode.Add(word.ToCharArray()));
        }

        /// </inheritdoc>
        public List<string> Search(string word)
        {
            var results = new ConcurrentBag<string>();

            RootNode.Search(word.ToCharArray(), string.Empty, results);
            return results.ToList();
        }
    }
}