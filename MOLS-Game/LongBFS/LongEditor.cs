using MOLS_Game.TreeClasses;
using System;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Linq;

namespace MOLS_Game.LongBFS
{
    public static class LongEditor
    {

        public static Dictionary<int, int> reversed = new Dictionary<int, int>
        {
            {1,3}, {2,2}, {3,1}
        };

        private static LongNode GenerateHorizontalLongMove(LongNode node, int num)
        {
            string[] tiles = node.GetTiles();
            int emptyIndex = node.GetEmptyIndex();

            int column = emptyIndex % 4;
            string[] output = (string[])tiles.Clone();
            int trueNum;
            int leftOrRight;

            if (num > column)
            {
                trueNum = reversed[num];
                leftOrRight = 1;
            } else
            {
                trueNum = num;
                leftOrRight = -1;
            }

            for (int i = 0; i < trueNum; i++)
            {
                string tempElement = output[emptyIndex + leftOrRight];
                output[emptyIndex + leftOrRight] = output[emptyIndex];
                output[emptyIndex] = tempElement;
                emptyIndex = emptyIndex + leftOrRight;
            }
            return new LongNode(output,node, emptyIndex,"h"+num.ToString(),false);


        }

        private static LongNode GenerateVerticalLongMove(LongNode node, int num)
        {
            string[] tiles = node.GetTiles();
            int emptyIndex = node.GetEmptyIndex();

            int row = emptyIndex / 4;
            string[] output = (string[])tiles.Clone();
            int trueNum;
            int leftOrRight;

            if (num > row)
            {
                trueNum = reversed[num];
                leftOrRight = 1;
            }
            else
            {
                trueNum = num;
                leftOrRight = -1;
            }

            for (int i = 0; i < trueNum; i++)
            {
                string tempElement = output[emptyIndex + 4*leftOrRight];
                output[emptyIndex + 4*leftOrRight] = output[emptyIndex];
                output[emptyIndex] = tempElement;
                emptyIndex = emptyIndex + 4 * leftOrRight;
            }
            return new LongNode(output,node, emptyIndex, "v"+num.ToString(),true);


        }
        
        public static string FindMOLSWithLongMoves(string[] tiles1, int target, int maxDepth)
        {
            int e = 0;

            for (int i = 0; i < tiles1.Length; i++)
            {
                if (tiles1[i] == "11")
                {
                    e = i; break;
                }
            }


            int n = 0;

            HashSet<string> checkedSet = new HashSet<string>();

            LongTree? tree = new LongTree(tiles1, e);

            Queue<LongNode> queue = new Queue<LongNode>();

            queue.Enqueue(tree.GetRoot());

            Stopwatch stopwatch1 = Stopwatch.StartNew();

            bool vertical;

            while (queue.Count != 0)
            {

                LongNode node = queue.Dequeue();



                //start of for console
                n++;
                if (n % 100000 == 0)
                {
                    int pathlength = node.GetOverallPath().Length/2;
                    GC.Collect();
                    Console.WriteLine("n: " + n + " Step: " + pathlength + " Time: " + stopwatch1.ElapsedMilliseconds + " QueueCount: " + queue.Count + " SetCount: " + checkedSet.Count);
                    stopwatch1.Restart();



                }
                //end of for console

                //return condition
                if (TileEditor.MOLSHeuristic(node.GetTiles())==target)
                {
                    return node.GetOverallPath();
                }
                if(node.GetOverallPath().Length/2 >= maxDepth)
                {
                    return "MOS_" + target + " not found within " + (maxDepth-1) + " long moves.";
                }






                vertical = !node.IsVertical;

                if (vertical ||node.GetParent() == null) //generate vertical
                {
                    for (int k = 1; k < 4; k++)
                    {
                        LongNode t = GenerateVerticalLongMove(node, k);

                        string joined = string.Join(",", t.GetTiles());
                        if (!checkedSet.Contains(joined))
                        {
                            checkedSet.Add(joined);
                            queue.Enqueue(t); // I don't even think that I need to set a the parent to have a child. it is a reverse tree

                            
                        }
                    }

                }
                if(!vertical || node.GetParent() == null) //generate horizontal
                {
                    for (int k = 1; k < 4; k++)
                    {
                        LongNode t = GenerateHorizontalLongMove(node, k);

                        string joined = string.Join(",", t.GetTiles());
                        if (!checkedSet.Contains(joined))
                        {
                            checkedSet.Add(joined);
                            queue.Enqueue(t); 
                            
                        }
                    }
                }





            }
            return "MOS_" + target + " not found within " + (maxDepth - 1) + " long moves."; 
        }





















        public static string FindBranchingFactorLongMoves(string[] tiles1, int maxDepth)
        {
            int e = 0;

            for (int i = 0; i < tiles1.Length; i++)
            {
                if (tiles1[i] == "11")
                {
                    e = i; break;
                }
            }


            int n = 0;

            HashSet<string> checkedSet = new HashSet<string>();

            LongTree? tree = new LongTree(tiles1, e);

            Queue<LongNode> queue = new Queue<LongNode>();

            int[] output = new int[maxDepth]; //index will be pathlength and value will be # of branches

            queue.Enqueue(tree.GetRoot());

            Stopwatch stopwatch1 = Stopwatch.StartNew();

            bool vertical;

            while (queue.Count != 0)
            {

                LongNode node = queue.Dequeue();
                int pathlength = node.GetOverallPath().Length / 2; //divided by 2 bec. each move has 2 chars


                //start of for console
                n++;
                if (n % 100000 == 0)
                {
                    
                    GC.Collect();
                    Console.WriteLine("n: " + n + " Step: " + pathlength + " Time: " + stopwatch1.ElapsedMilliseconds + " QueueCount: " + queue.Count + " SetCount: " + checkedSet.Count);
                    stopwatch1.Restart();



                }
                //end of for console

                //return condition
              
                if (pathlength >= maxDepth)
                {
                    string result1 = string.Join(", ", output);
                    Console.WriteLine(result1);
                    return result1;
                }


                vertical = !node.IsVertical;

                if (vertical || node.GetParent() == null) //generate vertical
                {
                    for (int k = 1; k < 4; k++)
                    {
                        LongNode t = GenerateVerticalLongMove(node, k);

                        string joined = string.Join(",", t.GetTiles());
                        if (!checkedSet.Contains(joined))
                        {
                            checkedSet.Add(joined);
                            queue.Enqueue(t); 
                            output[pathlength]++;

                        }
                    }

                }
                if (!vertical || node.GetParent() == null) //generate horizontal
                {
                    for (int k = 1; k < 4; k++)
                    {
                        LongNode t = GenerateHorizontalLongMove(node, k);

                        string joined = string.Join(",", t.GetTiles());
                        if (!checkedSet.Contains(joined))
                        {
                            checkedSet.Add(joined);
                            queue.Enqueue(t);
                            output[pathlength]++;
                        }
                    }
                }





            }
            string result = string.Join(", ", output);
            Console.WriteLine(result);
            return result;
        }






















        public static string FindMOLSWithLongMovesAlternating(string[] tiles1, int target, int maxDepth)
        {
            int e = 0;

            for (int i = 0; i < tiles1.Length; i++)
            {
                if (tiles1[i] == "11")
                {
                    e = i; break;
                }
            }


            int n = 0;

            HashSet<string> checkedSet = new HashSet<string>();

            if (tiles1 == null) throw new ArgumentNullException(nameof(tiles1));
            LongTree? tree = new LongTree(tiles1, e);

            Queue<LongNode> queue = new Queue<LongNode>();

            queue.Enqueue(tree.GetRoot());

            Stopwatch stopwatch1 = Stopwatch.StartNew();

            bool vertical;

            while (queue.Count != 0)
            {

                LongNode node = queue.Dequeue();



                //start of for console
                n++;
                if (n % 100000 == 0)
                {
                    int pathlength = node.GetOverallPath().Length;
                    GC.Collect();
                    Console.WriteLine("n: " + n + " Step: " + pathlength + " Time: " + stopwatch1.ElapsedMilliseconds + " QueueCount: " + queue.Count + " SetCount: " + checkedSet.Count);
                    stopwatch1.Restart();



                }
                //end of for console

                //return condition
                if (TileEditor.MOLSHeuristic(node.GetTiles()) == target)
                {
                    return node.GetOverallPath();
                }
                if (node.GetOverallPath().Length >= maxDepth)
                {
                    return "MOS_" + target + " not found within " + (maxDepth - 1) + " long moves.";
                }






                vertical = !node.IsVertical;

                if (vertical) //generate vertical
                {
                    for (int k = 1; k < 4; k++)
                    {
                        LongNode t = GenerateVerticalLongMove(node, k);

                        string joined = string.Join(",", t.GetTiles());
                        if (!checkedSet.Contains(joined))
                        {
                            checkedSet.Add(joined);
                            queue.Enqueue(t); // I don't even think that I need to set a the parent to have a child. it is a reverse tree


                        }
                    }

                }
                else //generate horizontal
                {
                    for (int k = 1; k < 4; k++)
                    {
                        LongNode t = GenerateHorizontalLongMove(node, k);

                        string joined = string.Join(",", t.GetTiles());
                        if (!checkedSet.Contains(joined))
                        {
                            checkedSet.Add(joined);
                            queue.Enqueue(t); // I don't even think that I need to set a the parent to have a child. it is a reverse tree


                        }
                    }
                }





            }
            return "";
        }

        public static Dictionary<int, int> GenerateBFSWithLongMoves(string[] tiles1, int depth)
        {
            int e = 0;

            for (int i = 0; i < tiles1.Length; i++)
            {
                if (tiles1[i] == "11")
                {
                    e = i; break;
                }
            }

            Dictionary<int, int> output = new Dictionary<int, int>();

            int n = 0;

            HashSet<string> checkedSet = new HashSet<string>();

            if (tiles1 == null) throw new ArgumentNullException(nameof(tiles1));
            LongTree? tree = new LongTree(tiles1, e);

            Queue<LongNode> queue = new Queue<LongNode>();

            queue.Enqueue(tree.GetRoot());

            Stopwatch stopwatch1 = Stopwatch.StartNew();

            bool vertical = true;

            while (queue.Count != 0)
            {

                LongNode node = queue.Dequeue();



                //start of for console
                n++;
                if (n % 100000 == 0)
                {
                    int pathlength = node.GetOverallPath().Length;
                    GC.Collect();
                    Console.WriteLine("n: " + n + " Step: " + pathlength + " Time: " + stopwatch1.ElapsedMilliseconds + " QueueCount: " + queue.Count + " SetCount: " + checkedSet.Count);
                    stopwatch1.Restart();



                }
                //end of for console

                //return condition
                if (node.GetOverallPath().Length >= depth)
                {
                    return output;
                }






                vertical = !node.IsVertical;

                if (vertical)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        LongNode t = GenerateVerticalLongMove(node, k);

                        string joined = string.Join(",", t.GetTiles());
                        if (!checkedSet.Contains(joined))
                        {
                            checkedSet.Add(joined);
                            queue.Enqueue(t); // I don't even think that I need to set a the parent to have a child. it is a reverse tree

                            int heuristic = TileEditor.MOLSHeuristic(t.GetTiles());
                            output[heuristic] = output.GetValueOrDefault(heuristic, 0) + 1;
                        }
                    }

                }
                else
                {
                    for (int k = 1; k < 4; k++)
                    {
                        LongNode t = GenerateHorizontalLongMove(node, k);

                        string joined = string.Join(",", t.GetTiles());
                        if (!checkedSet.Contains(joined))
                        {
                            checkedSet.Add(joined);
                            queue.Enqueue(t); // I don't even think that I need to set a the parent to have a child. it is a reverse tree

                            int heuristic = TileEditor.MOLSHeuristic(t.GetTiles());
                            output[heuristic] = output.GetValueOrDefault(heuristic, 0) + 1;
                        }
                    }
                }





            }
            return output;
        }


        public static string EvaluateHeuristicWithLongMoves(string[] tiles)
        {
            Dictionary<int, int> coincidences = GenerateBFSWithLongMoves(tiles, 14);
            string output = "";

            double total = 0;
            foreach (KeyValuePair<int, int> element in coincidences)
            {
                total += element.Value;

            }

            foreach (KeyValuePair<int, int> element in coincidences)
            {
                int heuristicValue = element.Key;
                int count = element.Value;

                if (heuristicValue <= 20)
                {
                    output += "MOS_" + heuristicValue + ": " + ((count)) + ",   \n";

                }
            }

            return output;

        }


    }
}
