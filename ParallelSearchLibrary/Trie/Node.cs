namespace ParallelSearchLibrary.Trie
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Node
    {
        // TODO don't need this, if base node and root node exist
        public Node()
        {
        }

        public Node(char[] word)
        {
            this.CheckWord(word);

            this.Character = word[0];
            if (word.Length == 1)
            {
                return;
            }

            this.Children.Add(new Node(word.Skip(1).ToArray()));
        }

        public char Character { get; }
        public List<Node> Children { get; } = new List<Node>();

        public void Add(char[] word)
        {
            this.CheckWord(word);

            // TODO base node needs pure .Add( new Node(word)) TODO "normal" node needs
            // this.Character = word[0];

            // TODO
            /*
            if (word.Length == 1)
            {
                return;
            }
            */

            var existingChild = Children.FirstOrDefault(child => child.Character == word[0]);
            if (existingChild != null)
            {
                existingChild.Add(word.Skip(1).ToArray());
            }
            else
            {
                this.Children.Add(new Node(word.Skip(1).ToArray()));
            }
        }

        public void Search(char[] word, string resultWord, List<string> result)
        {
            this.CheckWord(word);
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
                result.Add(resultWord);
            }
        }

        private void CheckWord(char[] word)
        {
            if (word == null || !word.Any()) throw new ArgumentException($"The '{nameof(word)}' char array is null or empty.");
        }
    }
}