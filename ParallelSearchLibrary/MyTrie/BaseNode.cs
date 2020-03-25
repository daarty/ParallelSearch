namespace ParallelSearchLibrary.MyTrie
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BaseNode
    {
        public List<Node> Children { get; } = new List<Node>();

        public void Add(char[] word)
        {
            this.CheckWord(word);

            var existingChild = Children.FirstOrDefault(child => child.Character == word[0]);
            if (existingChild != null)
            {
                existingChild.Add(word.Skip(1).ToArray());
            }
            else
            {
                this.Children.Add(new Node(word));
            }
        }

        public void Search(char[] word, string resultWord, List<string> result)
        {
            if (word == null) throw new ArgumentNullException($"The '{nameof(word)}' is null.");
            if (resultWord == null) throw new ArgumentNullException($"The '{nameof(resultWord)}' is null.");

            if (word.Any())
            {
                var existingChild = Children.FirstOrDefault(child => child.Character == word[0]);
                if (existingChild != null)
                {
                    existingChild.Search(word.Skip(1).ToArray(), resultWord + word[0], result);
                }
                else
                {
                    return;
                }
            }
            else if (!string.IsNullOrEmpty(resultWord))
            {
                this.GetAllWords(resultWord, result);
            }
        }

        protected void CheckWord(char[] word)
        {
            if (word == null || !word.Any()) throw new ArgumentException($"The '{nameof(word)}' char array is null or empty.");
        }

        protected void GetAllWords(string resultWord, List<string> result)
        {
            if (!this.Children.Any())
            {
                result.Add(resultWord);
                return;
            }

            foreach (var child in this.Children)
            {
                child.GetAllWords(resultWord + child.Character, result);
            }
        }
    }
}