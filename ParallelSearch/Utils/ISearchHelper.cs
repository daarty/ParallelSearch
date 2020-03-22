using System.Collections.Generic;

namespace ParallelSearch.Utils
{
    public interface ISearchHelper
    {
        List<string> SearchWithBasicTrie(List<string> wordList, string searchWord);
    }
}