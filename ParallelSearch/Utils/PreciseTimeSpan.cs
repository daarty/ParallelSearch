namespace ParallelSearch.Utils
{
    public class PreciseTimeSpan
    {
        public int Hours;
        public int Microseconds;
        public int Milliseconds;
        public int Minutes;
        public int Seconds;

        public string MillisecondsWithFractions => $"{Milliseconds}.{Microseconds:000}";
    }
}