using System;
using System.Collections.Generic;
using System.Linq;

namespace CalloutDialogueAPI
{
    /// <summary>
    /// Type of nodes, Dialogue, Prompt, Option, and Action.
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// node dialogue line said by npc
        /// </summary>
        NpcDialogue,
        ///
        /// node dialogue line said by player
        ///
        PlayerDialogue,
        /// <summary>
        /// node to initialize menu
        /// </summary>
        Prompt,
        /// <summary>
        /// node to add dialogue line to prompt node
        /// </summary>
        Option,
        /// <summary>
        /// node that runs an action
        /// </summary>
        Action
    }
    
    /// <summary>
    /// Main class for DAGDialogueSystem
    /// </summary>
    public class DirectedAcyclicGraph
    {
        /// <summary>
        /// The node class for the Directed Acyclic Graph.
        /// </summary>
        public class Node
        {
            private readonly Type _type;
            private readonly List<Edge> _edges;
            private readonly string _data;
            /// <summary>
            /// The method to be ran when in a action node
            /// </summary>
            public readonly Action Action;
            
            /// <summary>
            /// Makes a new node in memory
            /// </summary>
            /// <param name="type"> the type of node, NodeTypes.Type </param>
            /// <param name="data"> dialogue string </param>
            public Node(Type type, string data)
            {
                _type = type;
                _data = data;
                _edges = new List<Edge>();
            }

            /// <summary>
            /// Makes a new action node
            /// </summary>
            /// <param name="type"> the type of node, NodeTypes.Type </param>
            /// <param name="action"> method name wanting to run if this node is chosen randomly or directly chosen if its the only edge </param>
            private Node(Type type, Action action)
            {
                _type = type;
                _data = "[This is a Action Node]";
                _edges = new List<Edge>();
                Action = action;
            }
            
            /// <summary>
            /// Makes a new action node with dialogue
            /// </summary>
            /// <param name="type"> he type of node, NodeTypes.Type </param>
            /// <param name="action"> method name wanting to run if this node is chosen randomly or directly chosen if its the only edge </param>
            /// <param name="data"> dialogue line </param>
            private Node(Type type, Action action, string data)
            {
                _type = type;
                _data = data;
                _edges = new List<Edge>();
                Action = action;
            }
            
            /// <summary>
            /// Gets the node type.
            /// </summary>
            /// <returns>Int</returns>
            internal Type GetNType() { return _type; }

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
            /// <returns> Receiver of chosen edge </returns>
            internal Node GetNextNode()
            {
                if (EdgesCount() < 1) return null;

                var rand = new Random();
                var randomIndex = rand.Next(EdgesCount());
                return GetEdgeAtIndex(randomIndex).Receiver;
            }
            
            /// <summary>
            /// Adds edge between this node and a new node with the type and data params.
            /// If multiple AddNodes take place on the same node. The top (most left) node will be
            /// the first edge.
            /// </summary>
            /// <param name="type"> the type of node, NodeTypes.Type </param>
            /// <param name="data">dialogue line in string format</param>
            /// <returns> Node </returns>
            public Node AddNode(Type type, string data)
            {
                var node = new Node(type, data);
                ConnectTo(node);
                return node;
            }

            /// <summary>
            /// Adds a edge between this node and a new action node with no dialogue.
            /// </summary>
            /// <param name="method"> the method that needs to be ran</param>
            /// <returns></returns>
            public Node AddActionNode(Action method)
            {
                var node = new Node(Type.Action, method);
                ConnectTo(node);
                return node;
            }
            
            /// <summary>
            /// Adds a edge between this node and a new action node with dialogue.
            /// </summary>
            /// <param name="method"> the method that needs to be ran </param>
            /// <param name="data"> dialogue line in string format </param>
            /// <returns></returns>
            public Node AddActionNodeWithDialogue(Action method, string data)
            {
                var node = new Node(Type.Action, method, data);
                ConnectTo(node);
                return node;
            }

            /// <summary>
            /// Links n amount of nodes as parents (senders) of this node. 
            /// </summary>
            /// <param name="childNodes"> IEnumerable list of nodes </param>
            public void LinkNodesAsParents(IEnumerable<Node> childNodes)
            {
                foreach (var n in childNodes)
                {
                    var newEdge = new Edge(n, this);
                    n._edges.Add(newEdge);
                }
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