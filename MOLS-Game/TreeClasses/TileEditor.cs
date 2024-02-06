using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;
using System;
using System.Collections.Generic;

namespace MOLS_Game.TreeClasses
{
    public static class TileEditor
{
        
        public static string[]? GenerateUp(string[] tiles)
        {

            int emptyIndex = 0;

            for(int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] == "__") 
                {
                emptyIndex = i;
                }
            }

            if (emptyIndex > 3) // Check if not in the top row
            {
                return SwapTiles1(tiles, emptyIndex, emptyIndex - 4);
            }

            return null;
        }

        public static string[]? GenerateRight(string[] tiles)
        {

            int emptyIndex = 0;

            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] == "__")
                {
                    emptyIndex = i;
                }
            }

            if (emptyIndex % 4 != 3) // Check if not in the last column
            {
                return SwapTiles1(tiles, emptyIndex, emptyIndex + 1);
            }

            return null;
        }
        public static string[]? GenerateLeft(string[] tiles)
        {

            int emptyIndex = 0;

            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] == "__")
                {
                    emptyIndex = i;
                }
            }

            if (emptyIndex % 4 != 0) // Check if not in the first column
            {
                return SwapTiles1(tiles, emptyIndex, emptyIndex - 1);
            }

            return null;
        }

        public static string[]? GenerateDown(string[] tiles)
        {

            int emptyIndex = 0;

            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] == "__")
                {
                    emptyIndex = i;
                }
            }

            if (emptyIndex < 12) // Check if not in the bottom row
            {
                return SwapTiles1(tiles, emptyIndex, emptyIndex + 4);
            }

            return null;
        }

        private static string[] SwapTiles1(string[] tiles, int index1, int index2)
        {
            string[] output = (string[])tiles.Clone();

            var temp = output[index1];
            output[index1] = output[index2];
            output[index2] = temp;

            return output;
        }

        public static bool CheckIfMOLS(string[] tiles)
        {


            //columns
            Console.WriteLine(tiles.Length);
            for(int i = 0; i < 4; i++)
            {
                bool a = tiles[i].Substring(0, 1) == tiles[i + 4].Substring(0, 1);
                bool b = tiles[i + 4].Substring(0, 1) == tiles[i+8].Substring(0, 1);
                bool c = tiles[i + 8].Substring(0, 1) == tiles[i + 12].Substring(0, 1);
                
                if(a || b || c)
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
            for (int i = 0; i < 12; i+=4)
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


        public static string GetPermutations(string[] tiles1, int numberOfSteps)
        {
            if (tiles1 == null) throw new ArgumentNullException(nameof(tiles1));
            MOLSTree tree = new MOLSTree(tiles1);

            Queue<MOLSNode> queue = new Queue<MOLSNode>();

            queue.Enqueue(tree.GetRoot());

            for(int i = 0; i < Math.Pow(4,numberOfSteps+1)-1; i++) 
            {
            
                MOLSNode node = queue.Dequeue();
                string[] tiles = node.GetTiles();
                Console.WriteLine(tiles.Length);
                if (node != null && node.GetTiles() != null)
                {
                    if (node.IsMOLS())
                    {
                        return node.GetPath();
                    }

                    if(GenerateDown(tiles) != null)
                    {
                        node.SetDown(new MOLSNode(GenerateDown(tiles), "D"));
                        queue.Enqueue(node.GetDown());
                    }
                    
                    if(GenerateUp(tiles) != null)
                    {
                        node.SetUp(new MOLSNode(GenerateUp(tiles), "U"));
                        queue.Enqueue(node.GetUp());
                    }
                    
                    if(GenerateLeft(tiles) != null)
                    {
                        node.SetLeft(new MOLSNode(GenerateLeft(tiles), "L"));
                        queue.Enqueue(node.GetLeft());
                    }
                   
                    if(GenerateRight(tiles) != null)
                    {
                        node.SetRight(new MOLSNode(GenerateRight(tiles), "R"));
                        queue.Enqueue(node.GetRight());
                    }

                    
                }

            }
            return "No MOLS within" + numberOfSteps + "steps.";

        }



    }
}
