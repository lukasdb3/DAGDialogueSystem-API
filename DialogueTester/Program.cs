using System;
using System.Linq;
using static DAGDialogueSystem.DirectedAcyclicGraph;
namespace DialogueTester
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var root = new Node(1, "Root");
            
            // root Children
            var _1prompt = root.AddNode(2, "First Prompt Node");
            
            // _1prompt node children
            var _2 = _1prompt.AddNode(3, "Neutral Node");
            var _3 = _1prompt.AddNode(3, "Good Node");
            var _4 = _1prompt.AddNode(3, "Bad Node");
            var _5 = _1prompt.AddNode(3, "Sarcastic Node");
            
            Initialize(root);
        }

        public static void Initialize(Node root)
        {
            Console.WriteLine("---------- Dialogue starting! ----------");
            Console.WriteLine();
            Console.WriteLine();
            var cn = root;
            while (cn != null)
            {
                switch (cn.Type)
                {
                    case 1: // Ai Speaking
                        Console.WriteLine(cn.Data);
                        cn = cn.GetNextNode();
                        break;
                    case 2: // Menu Node
                        Console.WriteLine(cn.Data);
                        break;
                    case 3: // Menu Option
                        Console.WriteLine(cn.Data);
                        break;
                    case 4: // Stop Node
                        Console.WriteLine(cn.Data);
                        break;
                }
            }
        }
    }
}