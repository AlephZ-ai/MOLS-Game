using MOLS_Game.Components.Pages;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MOLS_Game.TreeClasses
{
    public class MOLSGenerator
    {
        private HashSet<string[]> generatedMOLS = new HashSet<string[]>(new StringArrayEqualityComparer());
        private int[] solutionLengths = new int[400];
        private int[] longMovesLengths = new int[400];
        private const int TargetMOLSCount = 385;

        public string GenerateAndExportMOLS(string[] initialTiles)
        {
            string[] tiles = initialTiles;

            while (generatedMOLS.Count < TargetMOLSCount)
            {
                tiles = TileEditor.RandomizeTiles(initialTiles);
                string[] mols = GetPermutationsHeuristicTemp(tiles);
                   
                if (!generatedMOLS.Contains(mols) && TileEditor.CheckIfMOLS(mols))
                {
                    generatedMOLS.Add(mols);
                    Console.WriteLine("MOLS: " + generatedMOLS.Count);
                }
            }
            int molsCheck = CheckIfUniqueAndMOLS(generatedMOLS);
            Console.WriteLine("MOLS Checked: " + molsCheck);
            string json = JsonConvert.SerializeObject(generatedMOLS);
            string filePath = "MOLS.json";
            File.WriteAllText(filePath, json);
            return json;
        }

        public string SolutionSample(string[] initialTiles)
        {
            for (int i = 0; i < TargetMOLSCount; i++)
            {
                string[] tiles = TileEditor.RandomizeTiles(initialTiles);
                string totalPath = TileEditor.GetPermutationsDaniel(tiles);
                string longMoves = Counter.ShortToLongMoves(totalPath);
                int longMovesCount = longMoves.Count(c => c == ',');
                int length = totalPath.Length;
                solutionLengths[i] = length;
                longMovesLengths[i] = longMovesCount;
                Console.WriteLine("Total Sampled: " + i);
            }
            string json = JsonConvert.SerializeObject(solutionLengths);
            string json1 = ConvertToJson(longMovesLengths);
            string filePath = "Lengths.json";
            File.WriteAllText(filePath, json);
            return json;
        }

        public string ConvertToJson(int[] array)
        {
            string json = JsonConvert.SerializeObject(array);
            string filePath = "Longs.json";
            File.WriteAllText(filePath, json);
            return json;
        }




        public static string[] GetPermutationsHeuristicTemp(string[] tiles1)
        {
            HashSet<string> checkedSet = new HashSet<string>();

            if (tiles1 == null) throw new ArgumentNullException(nameof(tiles1));
            MOLSTree? tree = new MOLSTree(tiles1);

            PriorityQueue<MOLSNode, int> queue = new PriorityQueue<MOLSNode, int>();

            queue.Enqueue(tree.GetRoot(), TileEditor.MOLSHeuristic(tiles1));

            while (queue.Count != 0)
            {

                MOLSNode node = queue.Dequeue();
                string[] tiles = node.GetTiles();
                string step = node.GetPath();

                //check if mols
                if (TileEditor.CheckIfMOLS(tiles))
                {
                    return tiles;
                }


                //start of generation
                if (!"U".Equals(step))
                {

                    string[] downNeighbor = TileEditor.GenerateDown(tiles);


                    if (downNeighbor != null)
                    {
                        string downNeighborJoined = string.Join(",", downNeighbor);
                        if (!checkedSet.Contains(downNeighborJoined))
                        {
                            checkedSet.Add(downNeighborJoined);
                            node.SetDown(new MOLSNode(downNeighbor, "D", node));
                            queue.Enqueue(node.GetDown(), TileEditor.MOLSHeuristic(downNeighbor));
                        }

                    }
                }

                if (!"D".Equals(step))
                {


                    string[] upNeighbor = TileEditor.GenerateUp(tiles);


                    if (upNeighbor != null)
                    {
                        string upNeighborJoined = string.Join(",", upNeighbor);
                        if (!checkedSet.Contains(upNeighborJoined))
                        {
                            checkedSet.Add(upNeighborJoined);
                            node.SetUp(new MOLSNode(upNeighbor, "U", node));
                            queue.Enqueue(node.GetUp(), TileEditor.MOLSHeuristic(upNeighbor));
                        }

                    }
                }

                if (!"R".Equals(step))
                {


                    string[] leftNeighbor = TileEditor.GenerateLeft(tiles);


                    if (leftNeighbor != null)
                    {
                        string leftNeighborJoined = string.Join(",", leftNeighbor);
                        if (!checkedSet.Contains(leftNeighborJoined))
                        {
                            checkedSet.Add(leftNeighborJoined);
                            node.SetLeft(new MOLSNode(leftNeighbor, "L", node));
                            queue.Enqueue(node.GetLeft(), TileEditor.MOLSHeuristic(leftNeighbor));
                        }

                    }
                }

                if (!"L".Equals(step))
                {
                    string[] rightNeighbor = TileEditor.GenerateRight(tiles);


                    if (rightNeighbor != null)
                    {
                        string rightNeighborJoined = string.Join(",", rightNeighbor);
                        if (!checkedSet.Contains(rightNeighborJoined))
                        {
                            checkedSet.Add(rightNeighborJoined);
                            node.SetRight(new MOLSNode(rightNeighbor, "R", node));
                            queue.Enqueue(node.GetRight(), TileEditor.MOLSHeuristic(rightNeighbor));
                        }

                    }
                }

                //end of generation



            }
            return GetPermutationsHeuristicTemp(TileEditor.RandomizeTiles(tiles1));

        }

        public static int CheckIfUniqueAndMOLS(HashSet<string[]> mols)
        {
            HashSet<string> checkedSet = new HashSet<string>();

            foreach (string[] m in mols)
            {
                string fullSequence = string.Join(";", m.Select(tile => string.Join(",", tile)));

                if (!checkedSet.Contains(fullSequence) && TileEditor.CheckIfMOLS(m))
                {
                    checkedSet.Add(fullSequence);
                }
            }
            return checkedSet.Count;
        }


    }

    public class StringArrayEqualityComparer : IEqualityComparer<string[]>
    {
        public bool Equals(string[] x, string[] y)
        {
            // Check if the references are the same (including both being null)
            if (ReferenceEquals(x, y)) return true;

            // If one is null, but not both, they're not equal
            if (x == null || y == null) return false;

            // If lengths are different, arrays are not equal
            if (x.Length != y.Length) return false;

            // Compare each element
            for (int i = 0; i < x.Length; i++)
            {
                if (!x[i].Equals(y[i])) return false;
            }

            return true;
        }

        public int GetHashCode(string[] obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            int hash = 17;
            // Compute hash code considering all elements in the array
            foreach (var s in obj)
            {
                hash = hash * 23 + (s != null ? s.GetHashCode() : 0);
            }
            return hash;
        }
    }


}
