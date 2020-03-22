﻿namespace ParallelSearch.Utils
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Gma.DataStructures.StringSearch;
    using log4net;

    public class SearchHelper : ISearchHelper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SearchHelper));

        public List<string> SearchWithBasicTrie(List<string> wordsList, string searchWord)
        {
            Logger.Debug($"Building the Trie with '{wordsList.Count}' elements...");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var trie = new Trie<string>();

            foreach (var word in wordsList)
            {
                trie.Add(word, word);
            }

            stopWatch.Stop();
            Logger.Debug($"Successfully built Trie with '{wordsList.Count}' elements in '{stopWatch.ElapsedMilliseconds}' ms.");

            Logger.Debug($"Searching for results in the Trie for '{searchWord}'...");
            stopWatch.Restart();

            var results = trie.Retrieve(searchWord);

            stopWatch.Stop();
            Logger.Debug($"Successfully found Trie with '{results.Count()}' results in '{stopWatch.ElapsedMilliseconds}' ms.");

            return results.ToList();
        }
    }
}