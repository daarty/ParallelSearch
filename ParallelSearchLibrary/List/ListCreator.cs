namespace ParallelSearchLibrary.List
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using log4net;
    using ParallelSearchLibrary.Result;
    using Timer;

    /// <summary>
    /// Implementation of the <see cref="IListCreator"/> interface.
    /// </summary>
    public class ListCreator : IListCreator
    {
        // TODO replace List<string> with char[] and new string[charArray]
        private static readonly char[] Characters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ListCreator));

        /// </inheritdoc>
        public WordListResult CreateWordList(int wordLength)
        {
            var wordList = new List<string>();

            if (wordLength == 0)
            {
                return new WordListResult { WordList = wordList, CreationTime = new PreciseTimeSpan() };
            }

            int numberOfWords = Convert.ToInt32(Math.Pow(Characters.Length, wordLength));
            var wordsArray = new string[numberOfWords];

            Logger.Debug($"Creating Word List with wordLength '{wordLength}'...");
            var timer = new PreciseTimer();
            timer.Start();

            for (int i = 0; i < numberOfWords; i++)
            {
                wordsArray[i] = GetRandomWord(wordLength);
            }

            var timeSpanCreation = timer.Stop();
            Logger.Debug($"Successfully created Word List with '{wordList.Count}' elements in '{timeSpanCreation}'.");

            var random = new Random();
            Logger.Debug($"Permutating Word List with '{wordList.Count}' elements...");
            timer.Start();

            wordList = wordsArray.OrderBy(x => random.Next()).ToList();

            var timeSpanShuffle = timer.Stop();
            Logger.Debug($"Successfully permutated Word List with '{wordList.Count}' elements in '{timeSpanShuffle}'.");

            return new WordListResult { WordList = wordList, CreationTime = new PreciseTimeSpan(timeSpanCreation.TotalMicroseconds + timeSpanShuffle.TotalMicroseconds) };
        }

        /// </inheritdoc>
        public WordListResult CreateWordListParallel(int wordLength)
        {
            var wordList = new List<string>();

            if (wordLength == 0)
            {
                return new WordListResult { WordList = wordList, CreationTime = new PreciseTimeSpan() };
            }

            int numberOfWords = Convert.ToInt32(Math.Pow(Characters.Length, wordLength));
            var wordsBag = new ConcurrentBag<string>();

            Logger.Debug($"Creating Word List with wordLength '{wordLength}'...");
            var timer = new PreciseTimer();
            timer.Start();

            Parallel.For(0, numberOfWords, x => wordsBag.Add(GetRandomWord(wordLength)));

            var timeSpanCreation = timer.Stop();
            Logger.Debug($"Successfully created Word List with '{wordList.Count}' elements in '{timeSpanCreation}'.");

            var random = new Random();
            Logger.Debug($"Permutating Word List with '{wordList.Count}' elements...");
            timer.Start();

            wordList = wordsBag.OrderBy(x => random.Next()).ToList();

            var timeSpanShuffle = timer.Stop();
            Logger.Debug($"Successfully permutated Word List with '{wordList.Count}' elements in '{timeSpanShuffle}'.");

            return new WordListResult { WordList = wordList, CreationTime = new PreciseTimeSpan(timeSpanCreation.TotalMicroseconds + timeSpanShuffle.TotalMicroseconds) };
        }

        private string GetRandomWord(int numberOfCharacters)
        {
            var random = new Random();

            var charArray = new char[numberOfCharacters];
            for (int i = 0; i < numberOfCharacters; i++)
            {
                charArray[i] = Characters[random.Next(Characters.Length)];
            }

            return new string(charArray);
        }
    }
}