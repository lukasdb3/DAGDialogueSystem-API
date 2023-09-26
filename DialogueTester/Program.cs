/*using System;
using System.Linq;
using static DAGDialogueSystem.DirectedAcyclicGraph;
namespace DialogueTester
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const string prefixS = "~r~Suspect:~w~ ";
            const string prefixP = "~y~You:~w~ ";
            var root = new Node(1, prefixP+"Excuse me, may I speak to you for a minute?");
            var _1 = root.AddNode(1, prefixS+"Sure of course officer..");
        
            var _2 = _1.AddNode(2, "Choose An Option");
            var _3 = _2.AddNode(3, "We have gotten reports of an intoxicated hiker.");
            var _4 = _2.AddNode(3, "You match the description of an intoxicated individual.");
            var _5 = _2.AddNode(3, "I know you have been drinking, how drunk are you?");
            var _6 = _2.AddNode(3, "Your going to get me drunk off your breath!");
                    
            //Ai Speaking Options
            //N
            var _7 = _3.AddNode(1, prefixS+"I can’t lie, I have had a few drinks but im good to walk.");
            var _8 = _3.AddNode(1, prefixS+"That’s not me officer, I swear!");
            //G
            var _9 = _4.AddNode(1, prefixS+"I think you are in the wrong area officer.. I'm not drunk!");
            var _10 = _4.AddNode(1, prefixS+"Get outta here! Your kidding! I'm walking my imaginary dog!");
            //B
            var _11 = _5.AddNode(1, prefixS+"Drinking is fun! I can tell your not a fun person..");
            var _12 = _5.AddNode(1, prefixS+"F*ck you, pig. Leave me alone!");
            //S
            var _13 = _6.AddNode(1, prefixS+"That’s why you want to talk to me?");
            var _14 = _6.AddNode(1, prefixS+"You should smell your own breath before you start judging.");
            
            
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
                        cn = cn.GetNextNode_Prompt();
                        break;
                    case 4: // Stop Node
                        Console.WriteLine(cn.Data);
                        break;
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Dialogue Done");
        }
    }
}*/