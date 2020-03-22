using System;
using log4net;

namespace ParallelSearch.Utils
{
    public class PreciseTimer
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TrieManager));

        private DateTime After { get; set; }
        private DateTime Before { get; set; }

        public void Start()
        {
            Before = DateTime.Now;
        }

        public PreciseTimeSpan Stop()
        {
            if (Before == null)
            {
                Logger.Warn("Before time was null. Return an empty PreciseTimeSpan instance.");
                return new PreciseTimeSpan();
            }

            After = DateTime.Now;
            var timeSpan = After.Subtract(Before);
            var microseconds = Convert.ToInt32(Math.Round(timeSpan.TotalSeconds * 1000000d)) % 1000;

            return new PreciseTimeSpan
            {
                Hours = timeSpan.Hours,
                Minutes = timeSpan.Minutes,
                Seconds = timeSpan.Seconds,
                Milliseconds = timeSpan.Milliseconds,
                Microseconds = microseconds
            };
        }
    }
}