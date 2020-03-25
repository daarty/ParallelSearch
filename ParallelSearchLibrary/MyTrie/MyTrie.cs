namespace ParallelSearchLibrary.MyTrie
{
    using System.Collections.Generic;

    public class MyTrie : IMyTrie
    {
        public MyTrie(List<string> wordList)
        {
            this.Create(wordList);
        }

        public BaseNode RootNode { get; } = new BaseNode();

        public void Create(List<string> wordList)
        {
            foreach (var word in wordList)
            {
                RootNode.Add(word.ToCharArray());
            }
        }

        public List<string> Search(string word)
        {
            var results = new List<string>();

            RootNode.Search(word.ToCharArray(), string.Empty, results);
            return results;
        }
    }
}