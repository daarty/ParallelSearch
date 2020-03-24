namespace ParallelSearchLibrary.Trie
{
    using System.Collections.Generic;
    using Gma.DataStructures.StringSearch;
    using ParallelSearchLibrary.Result;

    public interface ITrieManager
    {
        TrieCreationResult CreateTrie(TrieAlgorithm algorithm, List<string> wordList);

        TrieCreationResult CreateTrieParallel(TrieAlgorithm algorithm, List<string> wordList);

        SearchResult Search(ITrie<int> trie, string searchWord);
    }
}