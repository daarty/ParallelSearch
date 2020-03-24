namespace ParallelSearchLibrary.Trie
{
    using System.Collections.Generic;

    public class MyTrie : IMyTrie
    {
        public Node rootNode { get; } = new Node();

        public MyTrie(List<string> wordList)
        {
            this.Create(wordList);
        }

        public void Create(List<string> wordList)
        {
            foreach (var word in wordList)
            {
                rootNode.Add(word.ToCharArray());
            }
        }

        public List<string> Search(string word)
        {
            var results = new List<string>();
            // TODO root node without string.empty,and without char.
            rootNode.Search(word.ToCharArray(), string.Empty, results);
            return results;
        }
    }
}