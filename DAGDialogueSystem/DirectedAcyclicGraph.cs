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
            public Edge[] Edges;
            public string Data;

            // constructor for Node with the type & data only.
            public Node(int type, string data)
            {
                Type = type;
                Data = data;
            }
            
            // constructor for Node with the type & edges & data.
            public Node(int type, Edge[] edges, string data)
            {
                Type = type;
                Edges = edges;
                Data = data;
            }
            
            // function to add an edge at the next available slot in nodes Edges array.
            private void ConnectTo(Node target)
            {
                var position = 0;
                var edgesCount = Edges.Length;
                // if edges exist, get last edge plus one for position. if no edges, position equals zero.
                if (edgesCount != 0) position = edgesCount + 1;
                else position = 0;
                // creates the edge at position
                Edges[position] = new Edge(this, target);
            }
            
            // function that adds child to node, also connects the to nodes.
            public Node AddNode(int type, string data)
            {
                var node = new Node(type, data);
                ConnectTo(node);
                return node;
            }
        }

        /// <summary>
        /// Edge class is for arrows before nodes, sender is the parent node and receiver is the child node.
        /// </summary>
        public class Edge
        {
            public Node Sender;
            public Node Receiver;

            public Edge(Node sender, Node receiver)
            {
                Sender = sender;
                Receiver = receiver;
            }
        }
    }
}