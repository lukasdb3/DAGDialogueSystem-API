using System.Collections.Generic;
using System.Linq;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Drawing;
using LSPD_First_Response.Mod.API;
using static DAGDialogueSystem.DirectedAcyclicGraph;

namespace DAGDialogueSystem
{
    public class DialogueFunctions
    {
        private static int _waitTime = 5500;
        public static UIMenu DialogueMenu = new UIMenu("Dialogue Options", "");
        private static readonly MenuPool Pool = new MenuPool();
        private static bool _continueDialogue = true;
        private static Node _currentNode = null;
        
        /// <summary>
        ///  Iterates through dialogue starting with the root node.
        /// </summary>
        /// <param name="n"> root node </param>
        public static void IterateDialogue(Node n)
        {
            _currentNode = n;
            Pool.Add(DialogueMenu);
            while (_currentNode != null)
            {
                // yield game fiber so other things can run
                GameFiber.Yield();
                // process menu
                Pool.ProcessMenus();
                // check if the current callout is still running if not clean dialogue and return.
                if (!Functions.IsCalloutRunning())
                {
                    Clean();
                    return;
                }
                // if dialogue paused and menu is closed indicating player closed the option menu, redisplay it.
                if (!_continueDialogue && !DialogueMenu.Visible) DialogueMenu.Visible = true;
                // if dialogue paused skip the rest and do next iteration.
                if (!_continueDialogue) continue;
                Log.Info("ITERATE DIALOGUE FUNCTION", "Getting Node Type & Displaying!");
                switch (_currentNode.GetNType())
                {
                    case 1:
                        DisplayData(_currentNode.GetData());
                        GameFiber.Wait(_waitTime);
                        _currentNode = _currentNode.GetNextNode();
                        break;
                    case 2:
                        Log.Info("ITERATE DIALOGUE FUNCTION", "Going to CreateMenu() w/ current node");
                        _continueDialogue = false;
                        CreateMenu(_currentNode);
                        GameFiber.Wait(1000);
                        break;
                    case 3:
                        Log.Info("ITERATE DIALOGUE FUNCTION", "Displaying Player's Option!");
                        DisplayData("~y~You:~w~ "+_currentNode.GetData());
                        GameFiber.Wait(_waitTime);
                        _currentNode = _currentNode.GetNextNode();
                        break;
                }
            }
            Clean();
        }

        /// <summary>
        /// Displays the node data in a subtitle.
        /// </summary>
        /// <param name="s"> the node's data </param>
        private static void DisplayData(string s) { Game.DisplaySubtitle(s, 4000); }
        
        private static void CreateMenu(Node promptNode)
        {
            // clear menu for good luck
            DialogueMenu.Clear();
            // get all options from edges of promptNode
            var options = promptNode.GetEdges().Select(edge => edge.Receiver.GetData()).ToList();
            // shuffle them in any order
            Extensions.Shuffle(options);
            // add them to menu
            foreach (var option in options) DialogueMenu.AddItem(new UIMenuItem(option));
            
            // Customization
            var dialogueMenuTitleStyle = DialogueMenu.TitleStyle;
            dialogueMenuTitleStyle.Font = TextFont.Monospace;
            dialogueMenuTitleStyle.DropShadow = true;
            DialogueMenu.RefreshIndex();
            DialogueMenu.Width = 0.325f;
            DialogueMenu.AllowCameraMovement = false;

            DialogueMenu.Visible = true;
            Log.Info("CREATE MENU", "Menu Displayed!");
            DialogueMenu.OnItemSelect += DialogueMenuOnOnItemSelect;
        }

        private static void DialogueMenuOnOnItemSelect(UIMenu sender, UIMenuItem selecteditem, int index)
        {
            Log.Info("ON ITEM SELECT", "Selected Option = " + selecteditem.Text);
            DialogueMenu.Visible = false;
            Log.Info("ON ITEM SELECT", "Menu Hidden!");
            foreach (var node in _currentNode.GetEdges().Select(edge => edge.Receiver))
            {
                Game.LogTrivial(node.GetData() + " " + selecteditem.Text);
                if (selecteditem.Text != node.GetData()) continue;
                Log.Info("ON ITEM SELECT", "Selecting Option Node for Next Node!");
                _continueDialogue = true;
                _currentNode = node;
                return;
            }
        }

        /// <summary>
        /// Sets the global class variables back to default.
        /// </summary>
        public static void Clean()
        {
            Game.LogTrivial("Dialogue Cleaning Up!");
            DialogueMenu.Clear();
            Pool.Clear();
            _continueDialogue = true;
            _currentNode = null;
        }
    }
}