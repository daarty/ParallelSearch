namespace ParallelSearch.Utils
{
    using System;
    using System.Collections.Generic;
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
            var timeStampBefore = new DateTime();

            CreateWordListRecursive(wordLength, new List<string>(), wordsList);

            var timeStampAfter = new DateTime();
            var timeSpan = timeStampAfter.Subtract(timeStampBefore);
            logger.Debug($"Successfully created Word List with wordLength '{wordLength}' and '{wordsList.Count}' elements in '{timeSpan.TotalMilliseconds}' ms.");

            //// TODO Permutation doesn't work
            //var permutation = new Permutation(indices.ToArray());
            //var permutatedList = new List<string>();

            //foreach (var index in indices)
            //{
            //    permutatedList.Add(list[permutation[index]]);
            //}

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