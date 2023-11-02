using System;
using System.IO;

namespace DAGDialogueSystem
{
    /// <summary>
    /// Logs dialogue info to DAGDialogueSystemLOG
    /// </summary>
    public static class Logger
    {
        private static readonly string LogFileName = "DAGDialogueSystemLOG.txt";

        /// <summary>
        /// Deletes the DAGDialogueSystemLOG, the next Logger.Log will create a new one.
        /// </summary>
        public static void ResetLog()
        {
            File.Delete(LogFileName);
        }
        
        /// <summary>
        /// Logs a message to DAGDialgoueSystemLOG file.
        /// </summary>
        /// <param name="message"> the message</param>
        public static void Log(string message)
        {
            var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            File.AppendAllText(LogFileName, logMessage + Environment.NewLine);
        }
    }
}