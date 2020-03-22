namespace ParallelSearchLibrary.Trie
{
    using System.Collections.Generic;
    using System.Linq;
    using Gma.DataStructures.StringSearch;
    using log4net;
    using Timer;

    public class TrieManager : ITrieManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TrieManager));

        public ITrie<string> CreateTrie(TrieAlgorithm algorithm, List<string> wordList)
        {
            Logger.Debug($"Building the '{algorithm}' Trie with '{wordList.Count}' elements...");
            var timer = new PreciseTimer();
            timer.Start();

            ITrie<string> trie;

            switch (algorithm)
            {
                case TrieAlgorithm.Basic:
                    trie = new Trie<string>();
                    break;

                case TrieAlgorithm.Concurrent:
                    trie = new ConcurrentTrie<string>();
                    break;

                case TrieAlgorithm.Patricia:
                    trie = new PatriciaTrie<string>();
                    break;

                case TrieAlgorithm.Ukkonen:
                    trie = new UkkonenTrie<string>();
                    break;

                default:
                    Logger.Warn($"Unknown Trie Algorithm '{algorithm}'. Returning null.");
                    return null;
            }

            foreach (var word in wordList)
            {
                trie.Add(word, word);
            }

            var timeSpan = timer.Stop();
            Logger.Debug($"Successfully built '{algorithm}' Trie with '{wordList.Count}' elements in '{timeSpan}'.");

            return trie;
        }

        public List<string> Search(ITrie<string> trie, string searchWord)
        {
            var timer = new PreciseTimer();
            timer.Start();

            var results = trie.Retrieve(searchWord);

            var timeSpan = timer.Stop();
            Logger.Debug($"Successfully found Trie with '{results.Count()}' results in '{timeSpan}'.");

            return results.ToList();
        }
    }
}