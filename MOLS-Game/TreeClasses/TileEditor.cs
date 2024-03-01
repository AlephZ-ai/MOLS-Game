using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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




        //heuristic
        public static int MOLSHeuristic(string[] tiles)
        {
            
            int heuristicScore = 0;

            for (int i = 0; i < 4; i++)
            {
                HashSet<char> uniqueRowFirstDigits = new HashSet<char>();
                HashSet<char> uniqueColumnFirstDigits = new HashSet<char>();
                HashSet<char> uniqueRowSecondDigits = new HashSet<char>();
                HashSet<char> uniqueColumnSecondDigits = new HashSet<char>();

                for (int j = 0; j < 4; j++)
                {
                    string rowTile = tiles[i * 4 + j];
                    string columnTile = tiles[j * 4 + i];

                    uniqueRowFirstDigits.Add(rowTile[0]);
                    uniqueColumnFirstDigits.Add(columnTile[0]);
                    uniqueRowSecondDigits.Add(rowTile[1]);
                    uniqueColumnSecondDigits.Add(columnTile[1]);
                }

                heuristicScore += (4 - uniqueRowFirstDigits.Count) + (4 - uniqueColumnFirstDigits.Count) +
                                  (4 - uniqueRowSecondDigits.Count) + (4 - uniqueColumnSecondDigits.Count);
            }

            return heuristicScore; //returns lower score when it is closer to mols
        }





        public static string GetPermutations(string[] tiles1)
        {

            int n = 0;

            HashSet<string> checkedSet = new HashSet<string>();
            //Dictionary<string[], bool> checkedDict = new Dictionary<string[], bool>();


            if (tiles1 == null) throw new ArgumentNullException(nameof(tiles1));
            MOLSTree? tree = new MOLSTree(tiles1);

            PriorityQueue<MOLSNode,int> queue = new PriorityQueue<MOLSNode, int>();

            queue.Enqueue(tree.GetRoot(),0);

            Stopwatch stopwatch1 = Stopwatch.StartNew();

            while (queue.Count != 0) 
            {

                MOLSNode node = queue.Dequeue();
                string[] tiles = node.GetTiles();
                string step = node.GetPath();


                //start of for console
                n++;

                if (n == 100000)
                {
                    n=0;
                    Console.WriteLine("Step: " + node.GetOverallPath().Length + " Time: " + stopwatch1.ElapsedMilliseconds + " QueueCount: " + queue.Count + " SetCount: " + checkedSet.Count);
                    stopwatch1.Restart();
                    

                }
                //end of for console

                //check if mols
                if (CheckIfMOLS(tiles))
                {
                    return node.GetOverallPath();
                }


                //start of generation
                if (!"U".Equals(step))
                {

                    string[] downNeighbor = GenerateDown(tiles);
                    
 
                    if (downNeighbor != null) 
                    {
                        string downNeighborJoined = string.Join(",", downNeighbor);
                        if (!checkedSet.Contains(downNeighborJoined))
                        {
                            checkedSet.Add(downNeighborJoined);
                            node.SetDown(new MOLSNode(downNeighbor, "D", node));
                            queue.Enqueue(node.GetDown(),MOLSHeuristic(downNeighbor));
                        } 
                        
                    }
                }

                if (!"D".Equals(step))
                {


                    string[] upNeighbor = GenerateUp(tiles);
                    

                    if (upNeighbor != null)
                    {
                        string upNeighborJoined = string.Join(",", upNeighbor);
                        if (!checkedSet.Contains(upNeighborJoined))
                        {
                            checkedSet.Add(upNeighborJoined);
                            node.SetUp(new MOLSNode(upNeighbor, "U", node));
                            queue.Enqueue(node.GetUp(),MOLSHeuristic(upNeighbor));
                        }
                        
                    }
                }

                if (!"R".Equals(step))
                {


                    string[] leftNeighbor = GenerateLeft(tiles);
                    

                    if (leftNeighbor != null) 
                    {
                        string leftNeighborJoined = string.Join(",", leftNeighbor);
                        if (!checkedSet.Contains(leftNeighborJoined))
                        {
                            checkedSet.Add(leftNeighborJoined);
                            node.SetLeft(new MOLSNode(leftNeighbor, "L", node));
                            queue.Enqueue(node.GetLeft(),MOLSHeuristic(leftNeighbor));
                        }
                        
                    }
                }

                if (!"L".Equals(step))
                {
                    string[] rightNeighbor = GenerateRight(tiles);
                    

                    if (rightNeighbor != null)
                    {
                        string rightNeighborJoined = string.Join(",", rightNeighbor);
                        if (!checkedSet.Contains(rightNeighborJoined))
                        {
                            checkedSet.Add(rightNeighborJoined);
                            node.SetRight(new MOLSNode(rightNeighbor, "R", node));
                            queue.Enqueue(node.GetRight(),MOLSHeuristic(rightNeighbor));
                        }
                        
                    }
                }

                //end of generation
                    
                

            }
            return "No MOLS";

        }


        public static string GetPermutations2(string[] tiles1, string[] tiles2)
        {
            

            int n = 0;


            Dictionary<string[], bool> checkedDict = new Dictionary<string[], bool>();


            if (tiles1 == null) throw new ArgumentNullException(nameof(tiles1));
            MOLSTree? tree = new MOLSTree(tiles1);

            Queue<MOLSNode> queue = new Queue<MOLSNode>();

            queue.Enqueue(tree.GetRoot());


            while (queue.Count != 0)
            {
                n++;


                MOLSNode node = queue.Dequeue();
                string[] tiles = node.GetTiles();

                if (n % 50000 == 0)
                {
                    Console.WriteLine(node.GetOverallPath().Length);
                }


                if (!checkedDict.ContainsKey(tiles))
                {
                    checkedDict.Add(tiles, true);


                    string step = node.GetPath();



                    if (tiles.SequenceEqual(tiles2))
                    {
                        return node.GetOverallPath();
                    }

                    if (!"U".Equals(step))
                    {

                        string[] downNeighbor = GenerateDown(tiles);

                        if (downNeighbor != null && !checkedDict.ContainsKey(downNeighbor))
                        {

                            node.SetDown(new MOLSNode(downNeighbor, "D", node));
                            queue.Enqueue(node.GetDown());
                        }
                    }

                    if (!"D".Equals(step))
                    {


                        string[] upNeighbor = GenerateUp(tiles);
                        if (upNeighbor != null && !checkedDict.ContainsKey(upNeighbor))
                        {
                            node.SetUp(new MOLSNode(upNeighbor, "U", node));
                            queue.Enqueue(node.GetUp());
                        }
                    }

                    if (!"R".Equals(step))
                    {


                        string[] leftNeighbor = GenerateLeft(tiles);
                        if (leftNeighbor != null && !checkedDict.ContainsKey(leftNeighbor))
                        {
                            node.SetLeft(new MOLSNode(leftNeighbor, "L", node));
                            queue.Enqueue(node.GetLeft());
                        }
                    }

                    if (!"L".Equals(step))
                    {
                        string[] rightNeighbor = GenerateRight(tiles);
                        if (rightNeighbor != null && !checkedDict.ContainsKey(rightNeighbor))
                        {
                            node.SetRight(new MOLSNode(rightNeighbor, "R", node));
                            queue.Enqueue(node.GetRight());
                        }
                    }



                }

            }
            return "No MOLS";

        }






        //OLD VERSION OF GETPERMUTATIONS
        public static string GetPermutationsOld(string[] tiles1)
        {


            int n = 0;

            HashSet<string> checkedSet = new HashSet<string>();
            //Dictionary<string[], bool> checkedDict = new Dictionary<string[], bool>();


            if (tiles1 == null) throw new ArgumentNullException(nameof(tiles1));
            MOLSTree? tree = new MOLSTree(tiles1);

            Queue<MOLSNode> queue = new Queue<MOLSNode>();

            queue.Enqueue(tree.GetRoot());

            Stopwatch stopwatch1 = Stopwatch.StartNew();

            while (queue.Count != 0)
            {

                MOLSNode node = queue.Dequeue();
                string[] tiles = node.GetTiles();
                string step = node.GetPath();


                //start of for console
                n++;

                if (n == 100000)
                {
                    n = 0;
                    Console.WriteLine("Step: " + node.GetOverallPath().Length + " Time: " + stopwatch1.ElapsedMilliseconds + " QueueCount: " + queue.Count + " SetCount: " + checkedSet.Count);
                    stopwatch1.Restart();


                }
                //end of for console

                //check if mols
                if (CheckIfMOLS(tiles))
                {
                    return node.GetOverallPath();
                }


                //start of generation
                if (!"U".Equals(step))
                {

                    string[] downNeighbor = GenerateDown(tiles);


                    if (downNeighbor != null)
                    {
                        string downNeighborJoined = string.Join(",", downNeighbor);
                        if (!checkedSet.Contains(downNeighborJoined))
                        {
                            checkedSet.Add(downNeighborJoined);
                            node.SetDown(new MOLSNode(downNeighbor, "D", node));
                            queue.Enqueue(node.GetDown());
                        }

                    }
                }

                if (!"D".Equals(step))
                {


                    string[] upNeighbor = GenerateUp(tiles);


                    if (upNeighbor != null)
                    {
                        string upNeighborJoined = string.Join(",", upNeighbor);
                        if (!checkedSet.Contains(upNeighborJoined))
                        {
                            checkedSet.Add(upNeighborJoined);
                            node.SetUp(new MOLSNode(upNeighbor, "U", node));
                            queue.Enqueue(node.GetUp());
                        }

                    }
                }

                if (!"R".Equals(step))
                {


                    string[] leftNeighbor = GenerateLeft(tiles);


                    if (leftNeighbor != null)
                    {
                        string leftNeighborJoined = string.Join(",", leftNeighbor);
                        if (!checkedSet.Contains(leftNeighborJoined))
                        {
                            checkedSet.Add(leftNeighborJoined);
                            node.SetLeft(new MOLSNode(leftNeighbor, "L", node));
                            queue.Enqueue(node.GetLeft());
                        }

                    }
                }

                if (!"L".Equals(step))
                {
                    string[] rightNeighbor = GenerateRight(tiles);


                    if (rightNeighbor != null)
                    {
                        string rightNeighborJoined = string.Join(",", rightNeighbor);
                        if (!checkedSet.Contains(rightNeighborJoined))
                        {
                            checkedSet.Add(rightNeighborJoined);
                            node.SetRight(new MOLSNode(rightNeighbor, "R", node));
                            queue.Enqueue(node.GetRight());
                        }

                    }
                }

                //end of generation



            }
            return "No MOLS";

        }














    }
}
