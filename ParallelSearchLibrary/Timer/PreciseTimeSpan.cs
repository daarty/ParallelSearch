namespace ParallelSearchLibrary.Timer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implementation of an easy-to-use precise timespan.
    /// </summary>
    public class PreciseTimeSpan
    {
        /// <summary>
        /// Creates a new instance of <see cref="PreciseTimeSpan"/>.
        /// </summary>
        /// <remarks>This basic constructor is needed for an empty instance.</remarks>
        public PreciseTimeSpan()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="PreciseTimeSpan"/>.
        /// </summary>
        /// <param name="microseconds">The total amount of microseconds of the new timespan.</param>
        public PreciseTimeSpan(ulong microseconds)
        {
            this.TotalMicroseconds = microseconds;
        }

        /// <summary>
        /// Creates a new instance of <see cref="PreciseTimeSpan"/>.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> element to be converted.</param>
        public PreciseTimeSpan(TimeSpan timeSpan)
        {
            this.TotalMicroseconds =
                Convert.ToUInt64(Math.Round(timeSpan.TotalSeconds * 1_000_000d));
        }

        /// <summary>
        /// Gets the number of full days.
        /// </summary>
        public int Days => Convert.ToInt32(Math.Floor(TotalMicroseconds / 86_400_000_000d));

        /// <summary>
        /// Gets the number of full hours.
        /// </summary>
        public int Hours => Convert.ToInt32(Math.Floor(TotalMicroseconds / 3_600_000_000d) % 24);

        /// <summary>
        /// Gets the number of full microseconds.
        /// </summary>
        public int Microseconds => Convert.ToInt32(TotalMicroseconds % 1000);

        /// <summary>
        /// Gets the number of full milliseconds.
        /// </summary>
        public int Milliseconds => Convert.ToInt32(Math.Floor(TotalMicroseconds / 1000d) % 1000);

        /// <summary>
        /// Gets the number of full minutes.
        /// </summary>
        public int Minutes => Convert.ToInt32(Math.Floor(TotalMicroseconds / 60_000_000d) % 60);

        /// <summary>
        /// Gets the number of full seconds.
        /// </summary>
        public int Seconds => Convert.ToInt32(Math.Floor(TotalMicroseconds / 1000_000d) % 60);

        /// <summary>
        /// Gets the core-unit of this timespan. An unsigned long microsecond counter.
        /// </summary>
        public ulong TotalMicroseconds { get; set; } = 0;

        /// <summary>
        /// Calculates the average of all elements in the passed collection.
        /// </summary>
        /// <param name="collection">Collection of precise time spans.</param>
        /// <returns>The average of all passed precise time spans.</returns>
        public static PreciseTimeSpan Average(IEnumerable<PreciseTimeSpan> collection)
        {
            if (collection.Count() == 0)
            {
                return new PreciseTimeSpan();
            }
            else if (collection.Count() == 1)
            {
                return collection.First();
            }

            var total = collection.Aggregate((a, b) => new PreciseTimeSpan(a.TotalMicroseconds + b.TotalMicroseconds));
            return new PreciseTimeSpan(total.TotalMicroseconds / (ulong)collection.Count());
        }

        /// <summary>
        /// Returns a human-readable <see cref="string"/> representation of the time span.
        /// </summary>
        /// <returns><see cref="string"/> representation of the time span.</returns>
        public override string ToString()
        {
            if (Hours > 0)
            {
                return $"{Hours} h {Minutes} min {Seconds}.{Milliseconds:000}{Microseconds:000} s";
            }
            else if (Minutes > 0)
            {
                return $"{Minutes} min {Seconds}.{Milliseconds:000}{Microseconds:000} s";
            }
            else if (Seconds > 0)
            {
                return $"{Seconds}.{Milliseconds:000}{Microseconds:000} s";
            }
            else
            {
                return $"{Milliseconds}.{Microseconds:000} ms";
            }
        }
    }
}