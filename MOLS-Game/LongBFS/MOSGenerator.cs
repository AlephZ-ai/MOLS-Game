using MOLS_Game.TreeClasses;
using Newtonsoft.Json;

namespace MOLS_Game.LongBFS
{
    public static class MOSGenerator
{

        private static HashSet<string> generatedMOS = new HashSet<string>();
        private static int TargetMOSCount = 200;
        private static readonly string filePath = "MOS_4.json";

        public static string GenerateAndExportMOS(string[] tiles)
        {
            LoadHashSet();
            TargetMOSCount = generatedMOS.Count + 200;
            
            while (generatedMOS.Count < TargetMOSCount)
            {
                tiles = TileEditor.RandomizeTiles(tiles);
                string[] mos = TileEditor.GetPermutationsHeuristicInput(tiles,4);

                if (!generatedMOS.Contains(string.Join(",", mos)))
                {
                    generatedMOS.Add(string.Join(",", mos));

                    if (generatedMOS.Count % 100 == 0)
                    {
                        Console.WriteLine("MOS4: " + generatedMOS.Count);
                    }
                }
            }

            
                Console.WriteLine("MOS_4 Generated: " + generatedMOS.Count);
            
            //string filePath = "MOS_4.json";
            //File.WriteAllText(filePath, json);

            SaveHashSet();
            return JsonConvert.SerializeObject(generatedMOS);
        }

        private static void LoadHashSet()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                generatedMOS = JsonConvert.DeserializeObject<HashSet<string>>(json) ?? new HashSet<string>();
            }
        }

        private static void SaveHashSet()
        {
            string json = JsonConvert.SerializeObject(generatedMOS, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

    }
}
