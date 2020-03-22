namespace ParallelSearch.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using log4net;

    /// <summary>
    /// Implementation of the <see cref="IListCreator"/> interface.
    /// </summary>
    public class ListCreator : IListCreator
    {
        private static readonly List<string> Characters = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ListCreator));

        /// </inheritdoc>
        public List<string> CreateWordList(int wordLength)
        {
            var wordList = new List<string>();

            if (wordLength == 0)
            {
                return wordList;
            }

            Logger.Debug($"Creating Word List with wordLength '{wordLength}'...");
            var timer = new PreciseTimer();
            timer.Start();

            CreateWordListRecursive(wordLength, new List<string>(), wordList);

            var timeSpan = timer.Stop();
            Logger.Debug($"Successfully created Word List with '{wordList.Count}' elements in '{timeSpan.MillisecondsWithFractions}' ms.");

            var random = new Random();
            Logger.Debug($"Permutating Word List with '{wordList.Count}' elements...");
            timer.Start();

            wordList = wordList.OrderBy(x => random.Next()).ToList();

            timeSpan = timer.Stop();
            Logger.Debug($"Successfully permutated Word List with '{wordList.Count}' elements in '{timeSpan.MillisecondsWithFractions}' ms.");

            return wordList;
        }

        private void CreateWordListRecursive(int maxWordLength, List<string> currentWord, List<string> wordList)
        {
            foreach (var character in Characters)
            {
                currentWord.Add(character);

                if (maxWordLength == currentWord.Count)
                {
                    var stringBuilder = new StringBuilder();
                    foreach (var letter in currentWord)
                    {
                        stringBuilder.Append(letter);
                    }

                    wordList.Add(stringBuilder.ToString());
                }
                else
                {
                    CreateWordListRecursive(maxWordLength, currentWord, wordList);
                }

                currentWord.RemoveAt(currentWord.Count - 1);
            }
        }
    }
}