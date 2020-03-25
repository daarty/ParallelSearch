namespace ParallelSearchLibrary.MyTrie
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MyParallelTrie : IMyTrie
    {
        public MyParallelTrie(List<string> wordList)
        {
            this.Create(wordList);
        }

        public ParallelNode RootNode { get; } = new ParallelNode();

        public void Create(List<string> wordList)
        {
            Parallel.ForEach(wordList, word => RootNode.Add(word.ToCharArray()));
        }

        public List<string> Search(string word)
        {
            var results = new ConcurrentBag<string>();

            RootNode.Search(word.ToCharArray(), string.Empty, results);
            return results.ToList();
        }
    }
}