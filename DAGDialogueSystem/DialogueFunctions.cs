using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using LSPD_First_Response.Mod.API;
using static DAGDialogueSystem.DirectedAcyclicGraph;
using static DAGDialogueSystem.Type;
using LSPD_First_Response.Mod.Callouts;

namespace DAGDialogueSystem
{
    /// <summary>
    /// Class used to iterate through dialogue.
    /// </summary>
    public static class DialogueFunctions
    {
        private const int WaitTime = 5500;
        /// <summary>
        /// Menu used for all dialogue options.
        /// </summary>
        public static readonly UIMenu DialogueMenu = new UIMenu("Dialogue Options", "");
        private static readonly MenuPool Pool = new MenuPool();
        private static bool _continueDialogue = true;
        private static Node _currentNode = null;

        /// <summary>
        /// Iterates through dialogue starting with the root node.
        /// </summary>
        /// <param name="callout"> current callout </param>
        /// <param name="n"> root node </param>
        public static void IterateDialogue(Callout callout, Node n)
        {
            // log stuff
            Logger.ResetLog();
            Logger.Log("DAGDialogueSystem Version - " + Assembly.GetExecutingAssembly().GetName().Version);
            Logger.Log("-!!!- ----- New Working Callout is " + callout.ScriptInfo.Name.ToUpper() + " -!!!- -----");
            
            _currentNode = n;
            Pool.Add(DialogueMenu);
            while (_currentNode != null)
            {
                // yield game fiber so other things can run
                GameFiber.Yield();
                // process menu
                Pool.ProcessMenus();
                // check if the current callout is still running if not clean dialogue and return.
                if (!LSPD_First_Response.Mod.API.Functions.IsCalloutRunning())
                {
                    Logger.Log("The current callout has ended. Ending dialogue.");
                    Clean();
                    return;
                }
                // if dialogue paused and menu is closed indicating player closed the option menu, redisplay it.
                if (!_continueDialogue && !DialogueMenu.Visible)
                {
                    DialogueMenu.Visible = true;
                    Logger.Log("Prevented closing menu before choosing an option!");
                }
                // if dialogue paused skip the rest and do next iteration.
                if (!_continueDialogue) continue;
                
                // log stuff
                Logger.Log("Current Node Type = " + _currentNode.GetNType());
                Logger.Log("Current Node Data = " + _currentNode.GetData());
                Logger.Log("Current Node Edges Count = " + _currentNode.GetEdges().Count());
                if (_currentNode.GetNType() == Type.Action)  Logger.Log("Current Node Action = " + _currentNode.Action.Method.Name);
                
                switch (_currentNode.GetNType())
                {
                    case Dialogue:
                        Logger.Log("Displaying data -[ "+_currentNode.GetData()+" ]-");
                        DisplayData(_currentNode.GetData());
                        GameFiber.Wait(WaitTime);
                        _currentNode = _currentNode.GetNextNode();
                        Logger.Log("Processing Current Node Finished!\n" +
                                   "-------------------------------------------------");
                        break;
                    case Prompt:
                        _continueDialogue = false;
                        CreateMenu(_currentNode);
                        GameFiber.Wait(1000);
                        Logger.Log("Processing Current Node Finished!\n" +
                                   "-------------------------------------------------");
                        break;
                    case Option:
                        DisplayData("~y~You:~w~ "+_currentNode.GetData());
                        GameFiber.Wait(WaitTime);
                        _currentNode = _currentNode.GetNextNode();
                        Logger.Log("Processing Current Node Finished!\n" +
                                   "-------------------------------------------------");
                        break;
                    case Action:
                        _currentNode.Action();
                        Logger.Log("Processing Current Node Finished!\n" +
                                   "-------------------------------------------------");
                        return;
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
            Logger.Log("Cleared menu");
            // get all options from edges of promptNode
            var options = promptNode.GetEdges().Select(edge => edge.Receiver.GetData()).ToList();
            // shuffle them in any order
            Extensions.Shuffle(options);
            // add them to menu
            Logger.Log("Adding options to menu");
            foreach (var option in options)
            {
                Logger.Log(option);
                DialogueMenu.AddItem(new UIMenuItem(option));
            }
            
            // customization
            var dialogueMenuTitleStyle = DialogueMenu.TitleStyle;
            dialogueMenuTitleStyle.Font = TextFont.Monospace;
            dialogueMenuTitleStyle.DropShadow = true;
            DialogueMenu.RefreshIndex();
            DialogueMenu.Width = 0.325f;
            DialogueMenu.AllowCameraMovement = false;

            DialogueMenu.Visible = true;
            
            // wait for option to be selected
            DialogueMenu.OnItemSelect += DialogueMenuOnOnItemSelect;
        }

        private static void DialogueMenuOnOnItemSelect(UIMenu sender, UIMenuItem selecteditem, int index)
        {
            DialogueMenu.Visible = false;
            // go through each edge and get the option node that was choosen.
            foreach (var node in _currentNode.GetEdges().Select(edge => edge.Receiver))
            {
                Game.LogTrivial(node.GetData() + " " + selecteditem.Text);
                if (selecteditem.Text != node.GetData()) continue;
                Logger.Log("Option " + selecteditem.Text + " was selected!");
                _continueDialogue = true;
                _currentNode = node;
                return;
            }
        }
        
        /// <summary>
        /// Cleans up and resets the menu.
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