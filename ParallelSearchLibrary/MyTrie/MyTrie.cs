namespace ParallelSearchLibrary.MyTrie
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements the <see cref="IMyTrie"/> interface.
    /// </summary>
    public class MyTrie : IMyTrie
    {
        /// <summary>
        /// Creates a new instance of <see cref="MyParallelTrie"/>.
        /// </summary>
        /// <param name="wordList">List of words to be added to the trie.</param>
        public MyTrie(List<string> wordList)
        {
            this.Create(wordList);
        }

        /// <summary>
        /// Gets the root node of the Trie-tree.
        /// </summary>
        public BaseNode RootNode { get; } = new BaseNode();

        /// </inheritdoc>
        public void Create(List<string> wordList)
        {
            foreach (var word in wordList)
            {
                RootNode.Add(word.ToCharArray());
            }
        }

        /// </inheritdoc>
        public List<string> Search(string word)
        {
            var results = new List<string>();

            RootNode.Search(word.ToCharArray(), string.Empty, results);
            return results;
        }
    }
}