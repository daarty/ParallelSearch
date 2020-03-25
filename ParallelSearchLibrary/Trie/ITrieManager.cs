namespace ParallelSearchLibrary.Trie
{
    using System.Collections.Generic;
    using Gma.DataStructures.StringSearch;
    using Result;
    using Timer;

    public interface ITrieManager
    {
        ITrie<int> Trie { get; set; }
        TrieAlgorithm TrieAlgorithm { get; set; }

        PreciseTimeSpan CreateTrie(List<string> wordList);

        SearchResult Search(string searchWord, List<string> wordList);
    }
}