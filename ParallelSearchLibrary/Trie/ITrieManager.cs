namespace ParallelSearchLibrary.Trie
{
    using System.Collections.Generic;
    using Gma.DataStructures.StringSearch;
    using ParallelSearchLibrary.Result;

    public interface ITrieManager
    {
        TrieAlgorithm TrieAlgorithm { get; set; }
        ITrie<int> Trie { get; set; }

        TrieCreationResult CreateTrie(List<string> wordList);

        TrieCreationResult CreateTrieParallel(List<string> wordList);

        SearchResult Search(string searchWord);
    }
}