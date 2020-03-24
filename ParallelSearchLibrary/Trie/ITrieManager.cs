namespace ParallelSearchLibrary.Trie
{
    using System.Collections.Generic;
    using Gma.DataStructures.StringSearch;
    using Result;
    using Timer;

    public interface ITrieManager
    {
        TrieAlgorithm TrieAlgorithm { get; set; }
        ITrie<int> Trie { get; set; }

        PreciseTimeSpan CreateTrie(List<string> wordList);

        SearchResult Search(string searchWord);
    }
}