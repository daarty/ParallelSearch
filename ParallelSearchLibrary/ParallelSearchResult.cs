namespace ParallelSearchLibrary
{
    using System.Collections.Generic;
    using Timer;

    public class ParallelSearchResult
    {
        public List<string> Result { get; set; }
        public PreciseTimeSpan SearchTime { get; set; }
    }
}