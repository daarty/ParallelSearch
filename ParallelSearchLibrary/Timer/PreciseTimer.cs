namespace ParallelSearchLibrary.Timer
{
    using System;
    using log4net;

    public class PreciseTimer
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PreciseTimer));

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

            return new PreciseTimeSpan(After.Subtract(Before));
        }
    }
}