namespace ParallelSearchLibrary.Timer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PreciseTimeSpan
    {
        public ulong TotalMicroseconds = 0;

        public PreciseTimeSpan()
        {
        }

        public PreciseTimeSpan(ulong microseconds)
        {
            this.TotalMicroseconds = microseconds;
        }

        public PreciseTimeSpan(TimeSpan timeSpan)
        {
            this.TotalMicroseconds =
                Convert.ToUInt64(Math.Round(timeSpan.TotalSeconds * 1_000_000d));
        }

        public int Days => Convert.ToInt32(Math.Floor(TotalMicroseconds / 86_400_000_000d));
        public int Hours => Convert.ToInt32(Math.Floor(TotalMicroseconds / 3_600_000_000d) % 24);
        public int Microseconds => Convert.ToInt32(TotalMicroseconds % 1000);
        public int Milliseconds => Convert.ToInt32(Math.Floor(TotalMicroseconds / 1000d) % 1000);
        public int Minutes => Convert.ToInt32(Math.Floor(TotalMicroseconds / 60_000_000d) % 60);
        public int Seconds => Convert.ToInt32(Math.Floor(TotalMicroseconds / 1000_000d) % 60);

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