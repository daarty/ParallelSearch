namespace ParallelSearchLibrary.Timer
{
    using System;
    using log4net;

    /// <summary>
    /// Implementation of an easy-to-use precise timer.
    /// </summary>
    public class PreciseTimer
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PreciseTimer));

        private DateTime After { get; set; }
        private DateTime Before { get; set; }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            Before = DateTime.Now;
        }

        /// <summary>
        /// Stops the timer and returns the precise time.
        /// </summary>
        /// <returns>
        /// The precise measured time span or an empty time span if the timer was never started.
        /// </returns>
        public PreciseTimeSpan Stop()
        {
            if (Before == null)
            {
                Logger.Warn("Before time was null. Return an empty PreciseTimeSpan instance.");
                return new PreciseTimeSpan();
            }

            After = DateTime.Now;

            return new PreciseTimeSpan(After.Subtract(Before));
        }
    }
}