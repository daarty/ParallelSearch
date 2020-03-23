namespace ParallelSearchLibrary.Result
{
    using Gma.DataStructures.StringSearch;
    using Timer;

    public class TrieCreationResult
    {
        public PreciseTimeSpan CreationTime { get; set; }
        public ITrie<int> Trie { get; set; }
    }
}