using Rage;

namespace DAGDialogueSystem
{
    internal class Log
    {
        internal static void Info(string sender, string m)
        {
            Game.LogTrivial("DAGDIalogueSystem ["+sender+"] - " +m);
        }
    }
}