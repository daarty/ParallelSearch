namespace ParallelSearch.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using Gma.DataStructures.StringSearch;
    using log4net;

    public class TrieManager : ITrieManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TrieManager));

        public ITrie<string> CreateBasicTrie(List<string> wordList)
        {
            Logger.Debug($"Building the Basic Trie with '{wordList.Count}' elements...");
            var timer = new PreciseTimer();
            timer.Start();

            var trie = new Trie<string>();

            foreach (var word in wordList)
            {
                trie.Add(word, word);
            }

            var timeSpan = timer.Stop();
            Logger.Debug($"Successfully built BasicTrie with '{wordList.Count}' elements in '{timeSpan.MillisecondsWithFractions}' ms.");

            return trie;
        }

        public List<string> Search(ITrie<string> trie, string searchWord)
        {
            var timer = new PreciseTimer();
            timer.Start();

            var results = trie.Retrieve(searchWord);

            var timeSpan = timer.Stop();
            Logger.Debug($"Successfully found Trie with '{results.Count()}' results in '{timeSpan.MillisecondsWithFractions}' ms.");

            return results.ToList();
        }
    }
}