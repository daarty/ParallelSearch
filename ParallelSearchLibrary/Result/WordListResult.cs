namespace ParallelSearchLibrary.Result
{
    using System.Collections.Generic;
    using Timer;

    public class WordListResult
    {
        public PreciseTimeSpan CreationTime { get; set; }
        public List<string> WordList { get; set; }
    }
}