namespace ParallelSearchLibrary.Trie
{
    using System.Collections.Generic;
    using System.Linq;
    using Gma.DataStructures.StringSearch;
    using log4net;
    using MyTrie;
    using Result;
    using Timer;

    public class TrieManager : ITrieManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TrieManager));
        public IMyTrie MyTrie { get; set; }
        public ITrie<int> Trie { get; set; }
        public TrieAlgorithm TrieAlgorithm { get; set; } = TrieAlgorithm.MyTrie;

        public PreciseTimeSpan CreateTrie(List<string> wordList)
        {
            Logger.Debug($"Building the '{this.TrieAlgorithm}' Trie with '{wordList.Count}' elements...");
            var timer = new PreciseTimer();
            PreciseTimeSpan timeSpan;
            timer.Start();

            switch (this.TrieAlgorithm)
            {
                case TrieAlgorithm.MyTrie:
                    this.MyTrie = new MyTrie(wordList);

                    timeSpan = timer.Stop();
                    Logger.Debug($"Successfully built '{this.TrieAlgorithm}' Trie with '{wordList.Count}' elements in '{timeSpan}'.");

                    return timeSpan;

                case TrieAlgorithm.MyParallelTrie:
                    this.MyTrie = new MyParallelTrie(wordList);

                    timeSpan = timer.Stop();
                    Logger.Debug($"Successfully built '{this.TrieAlgorithm}' Trie with '{wordList.Count}' elements in '{timeSpan}'.");

                    return timeSpan;

                case TrieAlgorithm.Basic:
                    this.Trie = new Trie<int>();
                    break;

                case TrieAlgorithm.Concurrent:
                    this.Trie = new ConcurrentTrie<int>();
                    break;

                case TrieAlgorithm.Patricia:
                    this.Trie = new PatriciaTrie<int>();
                    break;

                case TrieAlgorithm.Ukkonen:
                    this.Trie = new UkkonenTrie<int>();
                    break;

                default:
                    Logger.Warn($"Unknown Trie Algorithm '{this.TrieAlgorithm}'. Returning null.");
                    return null;
            }

            for (int i = 0; i < wordList.Count; i++)
            {
                this.Trie.Add(wordList[i], i);
            }

            timeSpan = timer.Stop();
            Logger.Debug($"Successfully built '{this.TrieAlgorithm}' Trie with '{wordList.Count}' elements in '{timeSpan}'.");

            return timeSpan;
        }

        public SearchResult Search(string searchWord, List<string> wordList)
        {
            var results = new List<string>();
            var timer = new PreciseTimer();
            timer.Start();

            switch (this.TrieAlgorithm)
            {
                case TrieAlgorithm.MyTrie:
                case TrieAlgorithm.MyParallelTrie:
                    results = this.MyTrie.Search(searchWord);
                    break;

                default:
                    this.Trie.Retrieve(searchWord).ToList().ForEach(x => results.Add(wordList[x]));
                    break;
            }

            var timeSpan = timer.Stop();
            Logger.Debug($"Successfully found Trie with '{results.Count()}' results in '{timeSpan}'.");

            return new SearchResult { ResultList = results, SearchTime = timeSpan };
        }
    }
}