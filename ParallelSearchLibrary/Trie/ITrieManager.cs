﻿namespace ParallelSearchLibrary.Trie
{
    using System.Collections.Generic;
    using Gma.DataStructures.StringSearch;

    public interface ITrieManager
    {
        ITrie<string> CreateTrie(TrieAlgorithm algorithm, List<string> wordList);

        List<string> Search(ITrie<string> trie, string searchWord);
    }
}