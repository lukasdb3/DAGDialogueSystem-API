using System;
using System.Collections.Generic;
using System.Linq;

namespace DAGDialogueSystem
{
    public class DirectedAcyclicGraph
    {
        /// <summary>
        /// Node class that includes one line of dialogue and n amount of edges, also has a type 1-4.
        /// 1 = AI Speaking, 2 = Menu, 3 = Menu Options, 4 = Stop Node
        /// </summary>
        public class Node
        {
            public int Type { get; private set; }
            public List<Edge> Edges;
            public string Data;

            // constructor for Node with the type & data only.
            public Node(int type, string data)
            {
                Type = type;
                Data = data;
                Edges = new List<Edge>();
            }
            
            // constructor for Node with the type & edges & data.
            public Node(int type, List<Edge> edges, string data)
            {
                Type = type;
                Edges = edges;
                Data = data;
            }
            
            // function to add an edge at the next available slot in nodes Edges array.
            private void ConnectTo(Node target)
            {
                // define our new edge.
                var newEdge = new Edge(this, target);
                Console.WriteLine("Edge created: " + newEdge.Sender.Data + " ---> " + newEdge.Receiver.Data);
                Edges.Add(newEdge);
                Console.WriteLine("Successful in Adding Edge!");
            }
            
            /// function that adds child to node, also connects the two nodes.
            public Node AddNode(int type, string data)
            {
                var node = new Node(type, data);
                ConnectTo(node);
                return node;
            }
            
            /// <summary>
            /// Function to iterate through n node that yield returns children.
            /// </summary>
            /// <returns> child(s) node of this node </returns>
            public IEnumerable<Node> IterateChildren()
            {
                foreach (var e in Edges)
                {
                    yield return e.Receiver;
                }
            }
            
            /// returns the amount of edges that node has.
            public int EdgesCount() {return Edges.Count;}

            /// returns the first edge of node.
            public Edge GetFirstEdge() { return Edges.FirstOrDefault(); }

            /// gets edge at index.
            public Edge GetEdgeAtIndex(int i) { return Edges.ElementAt(i); }
            
            /// randomly chooses next edge and returns the receiver node.
            public Node GetNextNode()
            {
                var rand = new Random();
                Node node = null;
                if (EdgesCount() == 1)
                {
                    node = GetFirstEdge().Receiver;
                }
                else
                {
                    var i = rand.Next(0, EdgesCount());
                    node = GetEdgeAtIndex(i).Receiver;
                }
                return node;
            }
        }

        /// <summary>
        /// Edge class is for arrows before nodes, sender is the parent node and receiver is the child node.
        /// </summary>
        public class Edge
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