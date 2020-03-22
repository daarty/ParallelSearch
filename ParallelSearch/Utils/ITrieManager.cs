using System.Collections.Generic;
using Gma.DataStructures.StringSearch;

namespace ParallelSearch.Utils
{
    public interface ITrieManager
    {
        ITrie<string> CreateBasicTrie(List<string> wordList);

        List<string> Search(ITrie<string> trie, string searchWord);
    }
}