namespace ParallelSearchLibrary.Timer
{
    public class PreciseTimeSpan
    {
        public int Hours;
        public int Microseconds;
        public int Milliseconds;
        public int Minutes;
        public int Seconds;

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