using System.Collections.Generic;
using Gma.DataStructures.StringSearch;

namespace ParallelSearch.Utils
{
    public interface ITrieManager
    {
        ITrie<string> CreateBasicTrie(List<string> wordsList);

        List<string> Search(ITrie<string> trie, string searchWord);

        List<string> SearchWithBasicTrie(List<string> wordsList, string searchWord);
    }
}