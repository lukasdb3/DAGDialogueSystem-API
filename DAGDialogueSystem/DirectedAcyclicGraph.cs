using System;
using System.Collections.Generic;
using System.Linq;

namespace DAGDialogueSystem
{
    public class DirectedAcyclicGraph
    {
        public class Node
        {
            private int _type;
            private readonly List<Edge> _edges;
            private readonly string _data;
            
            /// <summary>
            /// Node contains one line of dialogue.
            /// </summary>
            /// <param name="type">1=Hard Dialogue (Said in every conversation).
            /// 2=Menu Node (Node where you want the user to choose dialogue choice.)
            /// 3=Menu Option (Option for parent type 2 class.)</param>
            /// <param name="data">dialogue line in string format.</param>
            public Node(int type, string data)
            {
                _type = type;
                _data = data;
                _edges = new List<Edge>();
            }
            
            /// <summary>
            /// Gets the node type.
            /// </summary>
            /// <returns>Returns node type.</returns>
            internal int GetNType() { return _type; }

            /// <summary>
            /// Gets the node data.
            /// </summary>
            /// <returns>Returns node data.</returns>
            internal string GetData() { return _data; }

            /// <summary>
            /// Gets node edges.
            /// </summary>
            /// <returns> returns list of edges.</returns>
            internal IEnumerable<Edge> GetEdges() { return _edges; }


            /// <summary>
            /// Gets the next node in the graph.
            /// </summary>
            /// <returns> if no edges, returns null. If one edge, return's receiver of that edge. Likewise, if more than one edge does the same thing but randomly
            /// gets the edge. </returns>
            internal Node GetNextNode()
            {
                var rand = new Random();
                Node node;
                if (EdgesCount() < 1) return null;
                if (EdgesCount() == 1) node = GetFirstEdge().Receiver;
                else
                {
                    var i = rand.Next(0, EdgesCount());
                    node = GetEdgeAtIndex(i).Receiver;
                }
                return node;
            }
            
            /// <summary>
            /// Adds edge between this node and a new node with the type and data params.
            /// If multiple AddNodes take place on the same node. The top (most left) node will be
            /// the first edge.
            /// </summary>
            /// <param name="type">1=Hard Dialogue (Said in every conversation).
            /// 2=Menu Node (Node where you want the user to choose dialogue choice.)
            /// 3=Menu Option (Option for parent type 2 class.)</param>
            /// <param name="data">dialogue line in string format.</param>
            /// <returns> returns the new node. </returns>
            public Node AddNode(int type, string data)
            {
                var node = new Node(type, data);
                ConnectTo(node);
                return node;
            }
            
            /// <summary>
            /// Connects this node and target node with edge.
            /// </summary>
            /// <param name="target"> node that needs connecting.</param>
            private void ConnectTo(Node target)
            {
                // define our new edge.
                var newEdge = new Edge(this, target);
                _edges.Add(newEdge);
            }

            /// returns the amount of edges that node has.
            private int EdgesCount() { return _edges.Count; }

            /// returns the first edge of node.
            private Edge GetFirstEdge() { return _edges.FirstOrDefault(); }

            /// gets edge at index.
            /// <param name="i"> the index of needed edge. </param>>
            private Edge GetEdgeAtIndex(int i) { return _edges.ElementAt(i); }
        }
        
        /// <summary>
        /// The arrow in between nodes.
        /// </summary>
        internal class Edge
        {
            public readonly Node Sender;
            public readonly Node Receiver;

            public Edge(Node sender, Node receiver)
            {
                Sender = sender;
                Receiver = receiver;
            }
        }
    }
}

/*/// displays menu options and prompts for input.
           public Node GetNextNode_Prompt()
           {
               var i = 1;
               var optionNodes = new Dictionary<int, Node>();
               Console.WriteLine();
               Console.WriteLine("-- Options --");
               foreach (var edge in _edges)
               {
                   optionNodes.Add(i, edge.Receiver);
                   i++;
               }
               optionNodes = Extensions.ShuffleValues(optionNodes);
               foreach (var node in optionNodes) Console.WriteLine(node.Key+") "+node.Value.Data);
               Console.WriteLine("Which do you pick?!\n>>");
               var input = int.Parse(Console.ReadLine());
               var chosenNode = optionNodes[input];
               return chosenNode.GetNextNode();
           }*/