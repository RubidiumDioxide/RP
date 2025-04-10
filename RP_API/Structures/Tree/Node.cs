using System.Text;
using System.Linq;


namespace RP_API.Structures.Tree
{
    public class Node
    {
        public string Value = null;
        public List<Node> children = new List<Node> ();
        public List<string> strings = new List<string> (); 

        public Node(string value)
        {
            this.Value = value;
        }

        public void AddString(string s)
        {
            strings.Add(s); 
        }

        public Node() { }

        public IEnumerable<string> PrintOut()
        { 
            StringBuilder result = new StringBuilder();

            Traverse(this, "", result);

            string trimmedResult = result.ToString().TrimEnd(',', ' ');

            return string.IsNullOrEmpty(trimmedResult) ?
                Enumerable.Empty<string>() : 
                trimmedResult.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        }

        private void Traverse(Node node, string path, StringBuilder result)
        {
            path += $"{node.Value} {node.strings[0]}";

            if (node.children.Count == 0)
                result.Append(path + ", ");
            else
            {
                foreach (var child in node.children)
                    Traverse(child, path + " -> ", result); 
            }
        }
    }
}
