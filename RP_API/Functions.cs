using Microsoft.AspNetCore.Http.HttpResults;
using RP_API.Structures.Tree;
using System.IO;
using System.Xml.Linq;

namespace RP_API
{
    public static class Functions
    {
        public static IEnumerable<string> ReadFrom(string filename)
        {
            string line;
            using (var reader = File.OpenText(filename))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        } 

        static Tree ParseLine(Tree tree, string line)
        {
            // Line Example: Яндекс.Директ:/ru 
            string adPlatformName = line.Split(':').ToArray()[0];
            string[] paths = (line.Split(':')[1]).Split(','); 
            
            for(int i = 0; i < paths.Length; i++)
            {
                string[] components = paths[i].TrimStart('/').Split('/');

                Node node = tree;

                if (node.Value == null)
                {
                    node.Value = components[0];
                }

                for (int j = 1; j < components.Length; j++)
                {
                    Node existingNode = node.children.FirstOrDefault(n => n.Value == components[j]);

                    if (existingNode == null)
                    {
                        Node newNode = new Node(components[j]);
                        node.children.Add(newNode);
                        node = newNode; 
                    } 
                    else
                        node = existingNode; 
                }

                node.AddString(adPlatformName);
            }

            return tree; 
        }

        public static Tree LoadIntoTree(string filename)
        {
            Tree tree = new Tree();

            foreach(string line in ReadFrom(filename))
                tree = ParseLine(tree, line); 
                                
            return tree; 
        }

        public static IEnumerable<string> getPlatforms(Tree tree, string path)
        {
            List<string> adPlatforms = new List<string>();

            //if (path.Contains("2%F"))
             //   path.Replace("2%F", "/"); 

            string[] components = path.TrimStart('/').Split("/");
            int len = components.Length; 

            Node node = tree;

            for(int i = 0; i < len; i++)
            {
                if(node.Value == components[i])
                {
                    foreach (string s in node.strings)
                        adPlatforms.Add(s); 
                }

                if(i + 1 < len)
                {
                    node = node.children.Where(n => n.Value == components[i + 1]).FirstOrDefault(); 

                    if(node == null)
                        break;   
                }
            }

            return adPlatforms; 
        }
    }
}
