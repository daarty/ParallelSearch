namespace ParallelSearchLibrary.MyTrie
{
    using System.Linq;

    public class Node : BaseNode
    {
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
    }
}