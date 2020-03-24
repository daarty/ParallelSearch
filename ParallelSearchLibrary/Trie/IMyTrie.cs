namespace ParallelSearchLibrary.Trie
{
    using System.Collections.Generic;

    public interface IMyTrie
    {
        void Create(List<string> wordList);

        List<string> Search(string word);
    }
}