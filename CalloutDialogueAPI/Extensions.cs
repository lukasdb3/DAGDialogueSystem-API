using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using Rage;
using RAGENativeUI;

namespace CalloutDialogueAPI
{
    internal static class Extensions
    {
        private static Random _random = new Random();

        public static void Shuffle<T>(List<T> list)
        {
            var n = list.Count;
            for (var i = n - 1; i > 0; i--)
            {
                var j = _random.Next(0, i + 1);
                Swap(list, i, j);
            }
        }

        private static void Swap<T>(IList<T> list, int i, int j)
        {
            (list[i], list[j]) = (list[j], list[i]);
        }

        internal static void ChangeMenuWidth(UIMenu menu)
        {
            if (Game.IsKeyDown(Keys.Right)) menu.Width += .1f;
            if (Game.IsKeyDown(Keys.Left)) menu.Width -= .1f;
            Game.DisplaySubtitle("~r~"+menu.Width);
        }
    }
}