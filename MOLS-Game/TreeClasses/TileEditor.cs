using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;
using System;
using System.Collections.Generic;

namespace MOLS_Game.TreeClasses
{
    public static class TileEditor
    {

        public static string[]? MoveTile(string[] tiles, bool isVertical, int steps)
        {
            int emptyIndex = Array.IndexOf(tiles, "__");
            if (emptyIndex == -1) return null;

            int targetIndex = CalculateTargetIndex(emptyIndex, isVertical, steps);
            if (targetIndex != -1)
            {
                return SwapTiles(tiles, emptyIndex, targetIndex);
            }

            return tiles;
        }

        private static int CalculateTargetIndex(int index, bool isVertical, int steps)
        {
            int row = index / 4;
            int col = index % 4;
            int newRow = row, newCol = col;

            if (isVertical)
            {
                newRow += steps == 3 ? -1 : (steps % 4 == 1 ? 1 : 0);
                if (newRow < 0 || newRow >= 4) return -1; // Invalid move vertically
            }
            else
            {
                newCol += steps == 3 ? -1 : (steps % 4 == 1 ? 1 : 0);
                if (newCol < 0 || newCol >= 4) return -1; // Invalid move horizontally
            }

            return newRow * 4 + newCol;
        }

        private static string[] SwapTiles(string[] tiles, int index1, int index2)
        {
            string[] newTiles = (string[])tiles.Clone();
            (newTiles[index1], newTiles[index2]) = (tiles[index2], tiles[index1]);
            return newTiles;
        }

        public static bool CheckIfMOLS(string[] tiles)
        {
            //columns
            //Console.WriteLine(tiles.Length);
            for (int i = 0; i < 4; i++)
            {
                bool a = tiles[i].Substring(0, 1) == tiles[i + 4].Substring(0, 1);
                bool b = tiles[i + 4].Substring(0, 1) == tiles[i + 8].Substring(0, 1);
                bool c = tiles[i + 8].Substring(0, 1) == tiles[i + 12].Substring(0, 1);

                if (a || b || c)
                {
                    return false;
                }

                bool a1 = tiles[i].Substring(1, 1) == tiles[i + 4].Substring(1, 1);
                bool b1 = tiles[i + 4].Substring(1, 1) == tiles[i + 8].Substring(1, 1);
                bool c1 = tiles[i + 8].Substring(1, 1) == tiles[i + 12].Substring(1, 1);

                if (a1 || b1 || c1)
                {
                    return false;
                }
            }

            //rows
            for (int i = 0; i < 12; i += 4)
            {
                bool a = tiles[i].Substring(0, 1) == tiles[i + 1].Substring(0, 1);
                bool b = tiles[i + 1].Substring(0, 1) == tiles[i + 2].Substring(0, 1);
                bool c = tiles[i + 2].Substring(0, 1) == tiles[i + 3].Substring(0, 1);

                if (a || b || c)
                {
                    return false;
                }

                bool a1 = tiles[i].Substring(1, 1) == tiles[i + 1].Substring(1, 1);
                bool b1 = tiles[i + 1].Substring(1, 1) == tiles[i + 2].Substring(1, 1);
                bool c1 = tiles[i + 2].Substring(1, 1) == tiles[i + 3].Substring(1, 1);

                if (a1 || b1 || c1)
                {
                    return false;
                }
            }

            return true;
        }

        public static string GetPermutations(string[] tiles, int numberOfSteps)
        {
            var seen = new HashSet<string> { string.Join(",", tiles) };
            Queue<(string[] Tiles, string Path)> queue = new Queue<(string[] Tiles, string Path)>();
            queue.Enqueue((tiles, ""));

            while (queue.Count > 0)
            {
                var (currentTiles, path) = queue.Dequeue();
                if (CheckIfMOLS(currentTiles))
                {
                    return path.TrimEnd(',', ' ');
                }

                for (int i = 1; i <= 3; i++)
                {
                    foreach (bool isVertical in new[] { true, false })
                    {
                        var newTiles = MoveTile(currentTiles, isVertical, i);
                        if (newTiles != null && seen.Add(string.Join(",", newTiles)))
                        {
                            string moveDescription = TranslateBinaryMoveToUserDirection(isVertical, i);
                            queue.Enqueue((newTiles, $"{path}{moveDescription}, "));
                        }
                    }
                }
            }

            return "No MOLS";
        }

        public static string TranslateBinaryMoveToUserDirection(bool isVertical, int steps)
        {
            if (isVertical) return steps == 3 ? "Up" : (steps == 1 ? "Down" : "");
            else return steps == 3 ? "Left" : (steps == 1 ? "Right" : "");
        }
    }
}