using Rage;

namespace DAGDialogueSystem
{
    internal static class Log
    {
        internal static void Info(string sender, string m)
        {
            Game.LogTrivial("DAGDialogueSystem ["+sender+"] - " +m);
        }
    }
}