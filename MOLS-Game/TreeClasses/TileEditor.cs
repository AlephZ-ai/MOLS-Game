using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;
using System;
using System.Collections.Generic;

namespace MOLS_Game.TreeClasses
{
    public static class TileEditor
{




        //GENREATE FUNCTIONS
        
        public static string[]? GenerateUp(string[] tiles)
        {

            int emptyIndex = 0;

            for(int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] == "11") 
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
                if (tiles[i] == "11")
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
                if (tiles[i] == "11")
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
                if (tiles[i] == "11")
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





        //SWAP FUNCTION

        public static string[] SwapTiles1(string[] tiles, int index1, int index2)
        {
            string[] output = (string[])tiles.Clone();

            var temp = output[index1];
            output[index1] = output[index2];
            output[index2] = temp;

            return output;
        }





        //CHECK FUNCTIONS

        public static bool CheckIfPermutation(string[] tiles, string[] end)
        {
            
            return tiles.SequenceEqual(end);



        }





        public static bool CheckIfMOLS(string[] tiles)
        {
            if (tiles.Length != 16) return false; 

            for (int i = 0; i < 4; i++)
            {
                HashSet<char> rowChars1 = new HashSet<char>();
                HashSet<char> colChars1 = new HashSet<char>();
                HashSet<char> rowChars2 = new HashSet<char>();
                HashSet<char> colChars2 = new HashSet<char>();

                for (int j = 0; j < 4; j++)
                {
                    // rows
                    if (!rowChars1.Add(tiles[i * 4 + j][0]) || !rowChars2.Add(tiles[i * 4 + j][1])) return false;
                    // columns
                    if (!colChars1.Add(tiles[j * 4 + i][0]) || !colChars2.Add(tiles[j * 4 + i][1])) return false;
                }
            }

            return true;


        }

        public static string GetInverse(string t, Dictionary<string,string> map)
        {
            

            return map.GetValueOrDefault(t);


        }


        public static string GetPermutations(string[] tiles1)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("U", "D");
            map.Add("D", "U");
            map.Add("R", "L");
            map.Add("L", "R");

            int n = 0;
            int step = 0;

            Dictionary<string[], bool> checkedDict = new Dictionary<string[], bool>();


            if (tiles1 == null) throw new ArgumentNullException(nameof(tiles1));
            MOLSTree? tree = new MOLSTree(tiles1);

            Queue<MOLSNode> queue = new Queue<MOLSNode>();

            queue.Enqueue(tree.GetRoot());
            

            while(queue.Count() != 0) 
            {
                n++;

                
                MOLSNode node = queue.Dequeue();
                string[] tiles = node.GetTiles();

                if (n%50000==0)
                {
                    int temp = step = node.GetPath().Length;
                    
                    if (step != temp)
                    {
                        step = temp;
                        Console.WriteLine(step);
                    }
                    
                }


                if (!checkedDict.ContainsKey(tiles))
                {
                    checkedDict.Add(tiles, true);


                    int l = node.GetPath().Length-1;


                    if (node != null && tiles != null)
                    {
                        if (node.IsMOLS())
                        {
                            return node.GetPath();
                        }

                        if (l==-1 || !"D".Equals(GetInverse(node.GetPath().Substring(l, 1), map)))
                        {



                            string[] downNeighbor = GenerateDown(tiles);

                            if (downNeighbor != null && !checkedDict.ContainsKey(downNeighbor))
                            {

                                node.SetDown(new MOLSNode(downNeighbor, node.GetPath() + "D"));
                                queue.Enqueue(node.GetDown());
                            }
                        }

                        if (l == -1 || !"U".Equals(GetInverse(node.GetPath().Substring(l, 1), map)))
                        {


                            string[] upNeighbor = GenerateUp(tiles);
                            if (upNeighbor != null && !checkedDict.ContainsKey(upNeighbor))
                            {
                                node.SetUp(new MOLSNode(upNeighbor, node.GetPath() + "U"));
                                queue.Enqueue(node.GetUp());
                            }
                        }

                        if (l == -1 || !"L".Equals(GetInverse(node.GetPath().Substring(l, 1), map)))
                        {


                            string[] leftNeighbor = GenerateLeft(tiles);
                            if (leftNeighbor != null && !checkedDict.ContainsKey(leftNeighbor))
                            {
                                node.SetLeft(new MOLSNode(leftNeighbor, node.GetPath() + "L"));
                                queue.Enqueue(node.GetLeft());
                            }
                        }

                        if (l == -1 || !"R".Equals(GetInverse(node.GetPath().Substring(l, 1), map)))
                        {
                            string[] rightNeighbor = GenerateRight(tiles);
                            if (rightNeighbor != null && !checkedDict.ContainsKey(rightNeighbor))
                            {
                                node.SetRight(new MOLSNode(rightNeighbor, node.GetPath() + "R"));
                                queue.Enqueue(node.GetRight());
                            }
                        }


                    }
                }

            }
            return "No MOLS";

        }








        public static string GetPermutations2(string[] tiles1, string[] end)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("U", "D");
            map.Add("D", "U");
            map.Add("R", "L");
            map.Add("L", "R");


            int n = 0;

            Dictionary<string[], bool> checkedDict = new Dictionary<string[], bool>();


            if (tiles1 == null) throw new ArgumentNullException(nameof(tiles1));
            MOLSTree? tree = new MOLSTree(tiles1);

            Queue<MOLSNode> queue = new Queue<MOLSNode>();

            queue.Enqueue(tree.GetRoot());


            while (queue.Count() != 0)
            {
                n++;


                MOLSNode node = queue.Dequeue();
                string[] tiles = node.GetTiles();

                if (Math.Log(n) / Math.Log(3) == Math.Floor(Math.Log(n) / Math.Log(3)))
                {
                    Console.WriteLine(node.GetPath().Length);
                }


                if (!checkedDict.ContainsKey(tiles))
                {
                    checkedDict.Add(tiles, true);


                    int l = node.GetPath().Length - 1;


                    if (node != null && tiles != null)
                    {
                        if (CheckIfPermutation(node.GetTiles(), end))
                        {
                            return node.GetPath();
                        }

                        if (l == -1 || !"D".Equals(GetInverse(node.GetPath().Substring(l, 1), map)))
                        {



                            string[] downNeighbor = GenerateDown(tiles);

                            if (downNeighbor != null && !checkedDict.ContainsKey(downNeighbor))
                            {

                                node.SetDown(new MOLSNode(downNeighbor, node.GetPath() + "D"));
                                queue.Enqueue(node.GetDown());
                            }
                        }

                        if (l == -1 || !"U".Equals(GetInverse(node.GetPath().Substring(l, 1), map)))
                        {


                            string[] upNeighbor = GenerateUp(tiles);
                            if (upNeighbor != null && !checkedDict.ContainsKey(upNeighbor))
                            {
                                node.SetUp(new MOLSNode(upNeighbor, node.GetPath() + "U"));
                                queue.Enqueue(node.GetUp());
                            }
                        }

                        if (l == -1 || !"L".Equals(GetInverse(node.GetPath().Substring(l, 1), map)))
                        {


                            string[] leftNeighbor = GenerateLeft(tiles);
                            if (leftNeighbor != null && !checkedDict.ContainsKey(leftNeighbor))
                            {
                                node.SetLeft(new MOLSNode(leftNeighbor, node.GetPath() + "L"));
                                queue.Enqueue(node.GetLeft());
                            }
                        }

                        if (l == -1 || !"R".Equals(GetInverse(node.GetPath().Substring(l, 1), map)))
                        {
                            string[] rightNeighbor = GenerateRight(tiles);
                            if (rightNeighbor != null && !checkedDict.ContainsKey(rightNeighbor))
                            {
                                node.SetRight(new MOLSNode(rightNeighbor, node.GetPath() + "R"));
                                queue.Enqueue(node.GetRight());
                            }
                        }


                    }
                }

            }
            return "No MOLS";

        }










    }
}
