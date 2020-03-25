namespace ParallelSearchLibrary.Result
{
    using System.Collections.Generic;
    using Timer;

    /// <summary>
    /// Object that contains the results of the creation of a word list.
    /// </summary>
    public class WordListResult
    {
        /// <summary>
        /// Gets or sets the precise search time.
        /// </summary>
        public PreciseTimeSpan CreationTime { get; set; }

        /// <summary>
        /// Gets the created list of words.
        /// </summary>
        public List<string> WordList { get; set; }
    }
}