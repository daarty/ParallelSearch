namespace ParallelSearchLibrary.Trie
{
    using System.Collections.Generic;
    using System.Linq;
    using Gma.DataStructures.StringSearch;
    using log4net;
    using ParallelSearchLibrary.Result;
    using Timer;

    public class TrieManager : ITrieManager
    {
        public ITrie<int> Trie { get; set; }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(TrieManager));

        public TrieCreationResult CreateTrie(List<string> wordList)
        {
            Logger.Debug($"Building the '{this.TrieAlgorithm}' Trie with '{wordList.Count}' elements...");
            var timer = new PreciseTimer();
            timer.Start();

            ITrie<int> trie;

            switch (this.TrieAlgorithm)
            {
                case TrieAlgorithm.Basic:
                    trie = new Trie<int>();
                    break;

                case TrieAlgorithm.Concurrent:
                    trie = new ConcurrentTrie<int>();
                    break;

                case TrieAlgorithm.Patricia:
                    trie = new PatriciaTrie<int>();
                    break;

                case TrieAlgorithm.Ukkonen:
                    trie = new UkkonenTrie<int>();
                    break;

                default:
                    Logger.Warn($"Unknown Trie Algorithm '{this.TrieAlgorithm}'. Returning null.");
                    return null;
            }

            for (int i = 0; i < wordList.Count; i++)
            {
                trie.Add(wordList[i], i);
            }

            var timeSpan = timer.Stop();
            Logger.Debug($"Successfully built '{this.TrieAlgorithm}' Trie with '{wordList.Count}' elements in '{timeSpan}'.");

            this.Trie = trie;

            return new TrieCreationResult { Trie = trie, CreationTime = timeSpan };
        }

        public TrieAlgorithm TrieAlgorithm { get; set; } = TrieAlgorithm.Basic;

        public TrieCreationResult CreateTrieParallel(List<string> wordList)
        {
            // TODO implement parallelism
            return CreateTrie(wordList);
        }

        public SearchResult Search(string searchWord)
        {
            var timer = new PreciseTimer();
            timer.Start();

            var results = this.Trie.Retrieve(searchWord);

            var timeSpan = timer.Stop();
            Logger.Debug($"Successfully found Trie with '{results.Count()}' results in '{timeSpan}'.");

            return new SearchResult { ResultIds = results.ToList(), SearchTime = timeSpan };
        }
    }
}