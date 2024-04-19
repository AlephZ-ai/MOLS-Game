using MOLS_Game.TreeClasses;
using Newtonsoft.Json;

namespace MOLS_Game.LongBFS
{
    public static class PathInfoGenerator
{

        private static List<int> pathLengthList = new List<int>();
        private static int TargetMOSCount = 3; // if you change this make sure to change on line 16
        private static readonly string filePath = "LongMovesToMOS4List.json";

        public static string GenerateAndExportMOS4PathLengths(string[] tiles)
        {
            LoadHashSet();
            TargetMOSCount = pathLengthList.Count + 3; //change this and the definition on line 10 if you want to afk
            const int maxDepth = 12; //MAX you should change this to as high as your pc can handle


            while (pathLengthList.Count < TargetMOSCount)
            {
                tiles = TileEditor.RandomizeTiles(tiles);
                string path = LongEditor.FindMOLSWithLongMoves(tiles,4,maxDepth);

                if (path.StartsWith("M"))
                {
                    pathLengthList.Add(-1); //-1 is our not found value
                } else
                {
                    pathLengthList.Add(path.Length / 2); // divide by 2 because the string has 2 chars per move
                }

               
                Console.WriteLine("MOS4 Paths: " + pathLengthList.Count);
                

            }

            Console.WriteLine("Path Lengths to MOS_4 Generated: " + pathLengthList.Count);

            

            SaveHashSet();
            return JsonConvert.SerializeObject(pathLengthList);
        }

        private static void LoadHashSet()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                pathLengthList = JsonConvert.DeserializeObject<List<int>>(json) ?? new List<int>();
            }
        }

        private static void SaveHashSet()
        {
            string json = JsonConvert.SerializeObject(pathLengthList, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

    }
}

