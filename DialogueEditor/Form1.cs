using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void btnAddDialogueNode_Click(object sender, EventArgs e)
        {
            // Create a new dialogue node and add it to the interface.
            var newNode = new DialogueNode();
            AddNodeToUI(newNode);
        }

        private void btnAddChoiceNode_Click(object sender, EventArgs e)
        {
            // Create a new choice node and add it to the interface.
            var newNode = new ChoiceNode();
            AddNodeToUI(newNode);
        }

        private void AddNodeToUI(Node node)
        {
            // Add the node to the UI, including setting its position on the form.
            // You can use labels, buttons, or any UI controls to represent nodes.
            var label = new Label
            {
                Text = node.GetType().Name, // Display the node type.
                Location = new System.Drawing.Point(100, 100), // Set the node's position.
            };
            this.Controls.Add(label);
        }

        private void btnConnectNodes_Click(object sender, EventArgs e)
        {
            // Implement node connection logic.
            // This could involve selecting two nodes and creating a connection between them.
        }

        // Define your Node, DialogueNode, and ChoiceNode classes.

        [Serializable]
        public class Node
        {
            public string Text { get; set; }
            // Add properties and methods as needed.
        }

        [Serializable]
        public class DialogueNode : Node
        {
            public string Speaker { get; set; }
            public string Emotion { get; set; }
            // Add dialogue-specific properties.
        }

        [Serializable]
        public class ChoiceNode : Node
        {
            public string[] Choices { get; set; }
            // Add choice-specific properties.
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Form1";
            this.ResumeLayout(false);
        }
    }
}
    
    
