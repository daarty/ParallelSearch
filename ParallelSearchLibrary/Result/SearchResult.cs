namespace ParallelSearchLibrary.Result
{
    using System.Collections.Generic;
    using Timer;

    /// <summary>
    /// Object that contains the search results.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets the list of results.
        /// </summary>
        public List<string> ResultList { get; set; }

        /// <summary>
        /// Gets or sets the precise search time.
        /// </summary>
        public PreciseTimeSpan SearchTime { get; set; }
    }
}