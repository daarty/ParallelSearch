namespace ParallelSearch.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using log4net;

    /// <summary>
    /// Implementation of the <see cref="IListCreator"/> interface.
    /// </summary>
    public class ListCreator : IListCreator
    {
        private static readonly List<string> Characters = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private static ILog logger = LogManager.GetLogger(typeof(ListCreator));

        /// </inheritdoc>
        public List<string> CreateWordList(int wordLength)
        {
            var wordsList = new List<string>();

            logger.Debug($"Creating Word List with wordLength '{wordLength}'...");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            CreateWordListRecursive(wordLength, new List<string>(), wordsList);

            stopWatch.Stop();
            logger.Debug($"Successfully created Word List with '{wordsList.Count}' elements in '{stopWatch.ElapsedMilliseconds}' ms.");

            var random = new Random();
            logger.Debug($"Permutating Word List with '{wordsList.Count}' elements...");
            stopWatch.Restart();

            wordsList = wordsList.OrderBy(x => random.Next()).ToList();

            stopWatch.Stop();
            logger.Debug($"Successfully permutated Word List with '{wordsList.Count}' elements in '{stopWatch.ElapsedMilliseconds}' ms.");

            return wordsList;
        }

        private void CreateWordListRecursive(int maxWordLength, List<string> currentWord, List<string> wordsList)
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

                    wordsList.Add(stringBuilder.ToString());
                }
                else
                {
                    CreateWordListRecursive(maxWordLength, currentWord, wordsList);
                }

                currentWord.RemoveAt(currentWord.Count - 1);
            }
        }
    }
}