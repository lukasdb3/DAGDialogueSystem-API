using System;
using System.Collections.Generic;
using System.Linq;

namespace DAGDialogueSystem
{
    /// <summary>
    /// Main class for DAGDialogueSystem
    /// </summary>
    public static class DirectedAcyclicGraph
    {
        /// <summary>
        /// The node class for the Directed Acyclic Graph.
        /// Type 1, Dialogue that is said in every conversation. This is usual a starter like "Hello Officer".
        /// Type 2, Prompt Node, initializes menu.
        /// Type 3, Dialogue Options, Parent must be Type 2.
        /// </summary>
        public class Node
        {
            private readonly int _type;
            private readonly List<Edge> _edges;
            private readonly string _data;
            
            /// <summary>
            /// Makes a new node in memory
            /// </summary>
            /// <param name="type">
            /// Type 1, Dialogue that is said in every conversation. This is usual a starter like "Hello Officer".
            /// Type 2, Prompt Node, initializes menu.
            /// Type 3, Dialogue Options, Parent must be Type 2. </param>
            /// <param name="data"> dialogue string </param>
            public Node(int type, string data)
            {
                _type = type;
                _data = data;
                _edges = new List<Edge>();
            }
            
            /// <summary>
            /// Gets the node type.
            /// </summary>
            /// <returns>Int</returns>
            internal int GetNType() { return _type; }

            /// <summary>
            /// Gets the node data.
            /// </summary>
            /// <returns>String</returns>
            internal string GetData() { return _data; }

            /// <summary>
            /// Gets node edges.
            /// </summary>
            /// <returns>List</returns>
            internal IEnumerable<Edge> GetEdges() { return _edges; }
            
            /// <summary>
            /// Gets the next node in the graph.
            /// </summary>
            /// <returns> if no edges, returns null. If one edge, return's receiver of that edge. Likewise, if more than one edge, does the same thing but randomly
            /// gets the edge to return. </returns>
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
            /// <param name="type"> 1 hard dialogue, 2 menu, 3 menu options</param>
            /// <param name="data">dialogue line in string format</param>
            /// <returns> Node </returns>
            public Node AddNode(int type, string data)
            {
                var node = new Node(type, data);
                ConnectTo(node);
                return node;
            }
            
            /// <summary>
            /// Connects this node and target node with edge.
            /// </summary>
            /// <param name="target"> node that needs connecting </param>
            public void ConnectTo(Node target)
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
            /// <param name="i"> the index of needed edge </param>>
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