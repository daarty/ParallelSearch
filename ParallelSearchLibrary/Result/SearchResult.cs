namespace ParallelSearchLibrary.Result
{
    using System.Collections.Generic;
    using Timer;

    public class SearchResult
    {
        public List<string> ResultList { get; set; }
        public PreciseTimeSpan SearchTime { get; set; }
    }
}