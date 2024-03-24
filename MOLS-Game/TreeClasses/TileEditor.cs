using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.ObjectPool;
using System.Collections;
using System.Xml.Linq;

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

        public static string[] RandomizeTiles(string[] tiles)
        {
            var rng = new Random();
            rng.Shuffle(tiles); //use this to shuffle for a shuffle button as well
            return tiles;
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




        //USES BREADTH FIRST SEARCH. TAKES AWHILE AND ONLY GOES 21~22 steps. 
        public static string GetPermutationsBreadthFirstSearch(string[] tiles1)
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
                if (n % 100000 == 0)
                {
                    GC.Collect();
                    Console.WriteLine("n: " + n + " Step: " + node.GetOverallPath().Length + " Time: " + stopwatch1.ElapsedMilliseconds + " QueueCount: " + queue.Count + " SetCount: " + checkedSet.Count);
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

        
        
        
        
        
        
        //USES A HEURISTIC. VERY FAST BUT GIVES A VERY LONG PATH
        public static string GetPermutationsHeuristic(string[] tiles1)
        {


            int n = 0;

            HashSet<string> checkedSet = new HashSet<string>();
            //Dictionary<string[], bool> checkedDict = new Dictionary<string[], bool>();


            if (tiles1 == null) throw new ArgumentNullException(nameof(tiles1));
            MOLSTree? tree = new MOLSTree(tiles1);

            PriorityQueue<MOLSNode, int> queue = new PriorityQueue<MOLSNode, int>();

            queue.Enqueue(tree.GetRoot(), MOLSHeuristic(tiles1));

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
                            queue.Enqueue(node.GetDown(), MOLSHeuristic(downNeighbor));
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
                            queue.Enqueue(node.GetUp(), MOLSHeuristic(upNeighbor));
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
                            queue.Enqueue(node.GetLeft(), MOLSHeuristic(leftNeighbor));
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
                            queue.Enqueue(node.GetRight(), MOLSHeuristic(rightNeighbor));
                        }

                    }
                }

                //end of generation



            }
            return "No MOLS";


        }




        //NOTE REMEMBER TO MAKE ONE SO THT THE CLOSER IT IS TO THE BEGINING THE BETTER THE PRIOIRTY
        public static string GetPermutationsIvan(string[] tiles1)
        {
            HashSet<string> checkedSet = new HashSet<string>();

            MOLSTree? tree = new MOLSTree(tiles1);

            PriorityQueue<MOLSNode, int> queue = new PriorityQueue<MOLSNode, int>();

            queue.Enqueue(tree.GetRoot(), MOLSHeuristic(tiles1));

            int n = 0;
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            while (queue.Count != 0)
            {
                MOLSNode node = queue.Dequeue();
                int priority = node.GetPriority();
                string[] tiles = node.GetTiles();
                string step = node.GetPath();

                //check if mols
                if (CheckIfMOLS(tiles))
                {
                    return node.GetOverallPath();
                }

                //start of for console
                n++;
                if (n % 100000 == 0)
                {
                    GC.Collect();
                    Console.WriteLine("n: " + n + " Step: " + node.GetOverallPath().Length + " Time: " + stopwatch1.ElapsedMilliseconds + " QueueCount: " + queue.Count + " SetCount: " + checkedSet.Count);
                    stopwatch1.Restart();
                }
                //end of for console

                int modifier = node.GetOverallPath().Length / 3;

                //start of generation
                if (!"U".Equals(step))
                {
                    string[] downNeighbor = GenerateDown(tiles);

                    if (downNeighbor != null)
                    {
                        string downNeighborJoined = string.Join(",", downNeighbor);
                        if (!checkedSet.Contains(downNeighborJoined))
                        {
                            int p = modifier * MOLSHeuristic(downNeighbor);

                            // Check if the last 3 moves were the same
                            if (node.GetOverallPath().Length >= 1 &&
                                node.GetOverallPath().Substring(node.GetOverallPath().Length - 1) == "D")
                            {
                                p *= 0;
                            }

                            checkedSet.Add(downNeighborJoined);
                            node.SetDown(new MOLSNode(downNeighbor, "D", node, p));
                            queue.Enqueue(node.GetDown(), p);
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
                            int p = modifier * MOLSHeuristic(upNeighbor);

                            // Check if the last 3 moves were the same
                            if (node.GetOverallPath().Length >= 1 &&
                                node.GetOverallPath().Substring(node.GetOverallPath().Length - 1) == "U")
                            {
                                p *= 0;
                            }

                            checkedSet.Add(upNeighborJoined);
                            node.SetUp(new MOLSNode(upNeighbor, "U", node, p));
                            queue.Enqueue(node.GetUp(), p);
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
                            int p = modifier * MOLSHeuristic(leftNeighbor);

                            // Check if the last 3 moves were the same
                            if (node.GetOverallPath().Length >= 1 &&
                                node.GetOverallPath().Substring(node.GetOverallPath().Length - 1) == "L")
                            {

                                p *= 0;
                            }

                            checkedSet.Add(leftNeighborJoined);
                            node.SetLeft(new MOLSNode(leftNeighbor, "L", node, p));
                            queue.Enqueue(node.GetLeft(), p);
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
                            int p = modifier * MOLSHeuristic(rightNeighbor);

                            // Check if the last 3 moves were the same
                            if (node.GetOverallPath().Length >= 1 &&
                                node.GetOverallPath().Substring(node.GetOverallPath().Length - 1) != "R")
                            {
                                p *= 15;
                            }

                            checkedSet.Add(rightNeighborJoined);
                            node.SetRight(new MOLSNode(rightNeighbor, "R", node, p));
                            queue.Enqueue(node.GetRight(), p);
                        }
                    }
                }
                //end of generation
            }
            return "No MOLS";
        }




        //idea I just wanted to try:

        public static string GetPermutationsDaniel(string[] tiles1)
        {


            HashSet<string> checkedSet = new HashSet<string>();

            MOLSTree? tree = new MOLSTree(tiles1);

            PriorityQueue<MOLSNode, int> queue = new PriorityQueue<MOLSNode, int>();

            queue.Enqueue(tree.GetRoot(), MOLSHeuristic(tiles1));

            int n = 0;
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            while (queue.Count != 0)
            {
                

                    MOLSNode node = queue.Dequeue();
                    int priority = node.GetPriority();
                    string[] tiles = node.GetTiles();
                    string step = node.GetPath();




                    //check if mols
                    if (CheckIfMOLS(tiles))
                    {
                        return node.GetOverallPath();
                    }
                    
                    //start of for console
                    n++;
                    if (n % 100000 == 0)
                    {
                        GC.Collect();
                        Console.WriteLine("n: " + n + " Step: " + node.GetOverallPath().Length + " Time: " + stopwatch1.ElapsedMilliseconds + " QueueCount: " + queue.Count + " SetCount: " + checkedSet.Count);
                        stopwatch1.Restart();


                    }
                    //end of for console



                int modifier = node.GetOverallPath().Length/3;




                    //start of generation
                    if (!"U".Equals(step))
                    {

                        string[] downNeighbor = GenerateDown(tiles);


                        if (downNeighbor != null)
                        {
                            string downNeighborJoined = string.Join(",", downNeighbor);
                            if (!checkedSet.Contains(downNeighborJoined))
                            {
                                int p = modifier * MOLSHeuristic(downNeighbor);
                                checkedSet.Add(downNeighborJoined);
                                node.SetDown(new MOLSNode(downNeighbor, "D", node, p));
                                queue.Enqueue(node.GetDown(), p);
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
                                int p = modifier * MOLSHeuristic(upNeighbor);
                                checkedSet.Add(upNeighborJoined);
                                node.SetUp(new MOLSNode(upNeighbor, "U", node, p));
                                queue.Enqueue(node.GetUp(), p);
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
                                int p = modifier * MOLSHeuristic(leftNeighbor);
                                checkedSet.Add(leftNeighborJoined);
                                node.SetLeft(new MOLSNode(leftNeighbor, "L", node, p));
                                queue.Enqueue(node.GetLeft(), p);
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
                                int p = modifier * MOLSHeuristic(rightNeighbor);
                                checkedSet.Add(rightNeighborJoined);
                                node.SetRight(new MOLSNode(rightNeighbor, "R", node, p));
                                queue.Enqueue(node.GetRight(), p);
                            }

                        }
                    }

                    //end of generation

                


            }
            return "No MOLS";


        }

















        //combination of breadth first search and heuristic to get what we are looking for. Maybe try IDA* or A* next time?
        public static string GetPermutationsNewest(string[] tiles1,int accuracy)
        {

            int n = 0;


            HashSet<string> checkedSet = new HashSet<string>();

            MOLSTree2 tree = new MOLSTree2(tiles1,GetPermutationsHeuristic(tiles1));
            
            MOLSNode2 shortestHPath = tree.GetRoot();
            //Console.WriteLine(shortestHPath.ToString());

            Queue<MOLSNode2> queue = new Queue<MOLSNode2>();

            queue.Enqueue(tree.GetRoot());

            Stopwatch stopwatch1 = Stopwatch.StartNew();

            while (queue.Count != 0)
            {

                MOLSNode2 node = queue.Dequeue();
                string[] tiles = node.GetTiles();
                string step = node.GetPath();


                //start of for console
                n++;

                if (n == 5)
                {
                    n = 0;
                    int pathLength = node.GetOverallPath().Length;

                    if (pathLength == accuracy)
                    {
                        //REMEMEBER TO MAKE IT SO THAT RL, LR, UD, and DU cancel out. 
                        return shortestHPath.GetOverallPath() + shortestHPath.HPath;
                    }
                    
                    Console.WriteLine("Step: " + pathLength + " Time: " + stopwatch1.ElapsedMilliseconds + " QueueCount: " + queue.Count + " SetCount: " + checkedSet.Count);
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
                            

                            string tempHPath = GetPermutationsHeuristic(downNeighbor);

                            if(tempHPath.Length < shortestHPath.HPath.Length)
                            {
                                node.SetDown(new MOLSNode2(downNeighbor, "D", node, tempHPath));
                                shortestHPath = node.GetDown();
                            } else
                            {
                                node.SetDown(new MOLSNode2(downNeighbor, "D", node));
                            }
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

                            string tempHPath = GetPermutationsHeuristic(upNeighbor);

                            if (tempHPath.Length < shortestHPath.HPath.Length)
                            {
                                node.SetUp(new MOLSNode2(upNeighbor, "U", node, tempHPath));
                                shortestHPath = node.GetUp();
                            }
                            else
                            {
                                node.SetUp(new MOLSNode2(upNeighbor, "U", node));
                            }
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

                            string tempHPath = GetPermutationsHeuristic(leftNeighbor);
                            if (tempHPath.Length < shortestHPath.HPath.Length)
                            {
                                node.SetLeft(new MOLSNode2(leftNeighbor, "L", node, tempHPath));
                                shortestHPath = node.GetLeft();
                            }
                            else
                            {
                                node.SetLeft(new MOLSNode2(leftNeighbor, "L", node));
                            }
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
                            string tempHPath = GetPermutationsHeuristic(rightNeighbor);
                            if (tempHPath.Length < shortestHPath.HPath.Length)
                            {
                                node.SetRight(new MOLSNode2(rightNeighbor, "R", node, tempHPath));
                                shortestHPath = node.GetRight();
                            }
                            else
                            {
                                node.SetRight(new MOLSNode2(rightNeighbor, "R", node));
                            }
                            queue.Enqueue(node.GetRight());
                        }

                    }
                }

                //end of generation



            }
            return "No MOLS";

        }






        //FAILED AND DEPRECIATED:::::





        public static List<MOLSNode> GetNodesOfDepthX(MOLSNode n, int depth, HashSet<string> checkedSet)
        {
            //HashSet<string> checkedSet = new HashSet<string>();

            string[] tiles1 = n.GetTiles();
            List<MOLSNode> nodesOfDepthX = new List<MOLSNode>();
            Queue<MOLSNode> queue = new Queue<MOLSNode>();

            checkedSet.Remove(string.Join(",", tiles1));

            queue.Enqueue(n);

            int currentTrueDepth = n.GetOverallPath().Length;


            while (queue.Count != 0)
            {

                MOLSNode node = queue.Dequeue();
                string[] tiles = node.GetTiles();
                string step = node.GetPath();




                //check if mols returns a List of only the mols if it is
                if (CheckIfMOLS(tiles))
                {
                    nodesOfDepthX.Clear();
                    nodesOfDepthX.Add(node);
                    return nodesOfDepthX;
                }
                //check if of depth x
                if (node.GetOverallPath().Length - currentTrueDepth == depth)
                {
                    nodesOfDepthX.Add(node);
                }
                else if (node.GetOverallPath().Length - currentTrueDepth > depth)
                {

                    return nodesOfDepthX;
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
            return nodesOfDepthX;


        }
        //DOESNT WORK EITHER
        public static string GetPermutationsIvan1FAIL(string[] tiles1, int depth)
        {
            MOLSTree tree = new MOLSTree(tiles1);
            MOLSNode currentNode = tree.GetRoot();
            List<MOLSNode> nodesAtDepth;
            HashSet<string> checkedSet = new HashSet<string>();
            Queue<MOLSNode> nodeQueue = new Queue<MOLSNode>();

            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            int n = 0;

            while (true)
            {
                n++;
                if (n % 100000 == 0)
                {
                    Console.WriteLine("Count: " + n / 100000 + " Time: " + stopwatch1.ElapsedMilliseconds + "ms Steps: " + currentNode.GetOverallPath().Length);
                    stopwatch1.Restart();
                }

                nodesAtDepth = GetNodesOfDepthX(currentNode, depth, checkedSet).OrderBy(n => MOLSHeuristic(n.GetTiles())).ToList();

                foreach (MOLSNode p in nodesAtDepth)
                {
                    nodeQueue.Enqueue(p);
                }
                if (nodesAtDepth.OrderBy(n => MOLSHeuristic(n.GetTiles())).FirstOrDefault() != null)
                {
                    currentNode = nodesAtDepth.OrderBy(n => MOLSHeuristic(n.GetTiles())).First();

                }
                else
                {
                    currentNode = nodeQueue.Dequeue();
                }

                if (CheckIfMOLS(currentNode.GetTiles()))
                {
                    return currentNode.GetOverallPath();
                }


            }
        }



        //FAILED
        public static string GetPermutationsIvan2FAIL(string[] tiles, int depth)
        {
            MOLSTree tree = new MOLSTree(tiles);

            List<MOLSNode> frontierNodes = new List<MOLSNode>();
            HashSet<string> checkedSet = new HashSet<string>();
            Queue<MOLSNode> bfsQueue = new Queue<MOLSNode>();
            //PriorityQueue<MOLSNode,int> pQueue = new PriorityQueue<MOLSNode, int>();

            bfsQueue.Enqueue(tree.GetRoot());

            while (true)
            {
                int currentDepth = 0;
                while (bfsQueue.Count != 0 && currentDepth < depth)
                {

                    MOLSNode node = bfsQueue.Dequeue();
                    string step = node.GetPath();
                    currentDepth = node.GetOverallPath().Length;

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
                                bfsQueue.Enqueue(node.GetDown());
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
                                bfsQueue.Enqueue(node.GetUp());
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
                                bfsQueue.Enqueue(node.GetLeft());
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
                                bfsQueue.Enqueue(node.GetRight());
                            }

                        }
                    }



                    if (currentDepth == depth - 1)
                    {
                        Console.WriteLine("TEST" + bfsQueue.Count);
                        frontierNodes.AddRange(bfsQueue);
                        bfsQueue.Clear();
                    }






                }
                Console.WriteLine(frontierNodes.Count);

                MOLSNode t = frontierNodes.OrderBy(n => MOLSHeuristic(n.GetTiles())).First();
                return t.GetOverallPath() + GetPermutationsHeuristic(t.GetTiles());

                frontierNodes = frontierNodes.OrderBy(n => MOLSHeuristic(n.GetTiles())).ToList();

                foreach (MOLSNode node in frontierNodes)
                {
                    bfsQueue.Enqueue(node);

                }
            }
        }

        //find path from 1 permutation to another. very old as it uses a dictionary instead of hashset
        //maybe after changing it we can use it to get from one point to another on 
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







    }
}
