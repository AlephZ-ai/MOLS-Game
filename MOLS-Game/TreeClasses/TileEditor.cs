using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;

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
                SwapTiles1(tiles, emptyIndex, emptyIndex + 1);
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
                SwapTiles1(tiles, emptyIndex, emptyIndex - 1);
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
                SwapTiles1(tiles, emptyIndex, emptyIndex + 4);
            }

            return null;
        }

        private static string[] SwapTiles1(string[] tiles, int index1, int index2)
        {
            string[] output = tiles;

            var temp = output[index1];
            output[index1] = output[index2];
            output[index2] = temp;

            return output;
            
            
        }

        public static bool CheckIfMOLS(string[] tiles)
        {
            

            //columns
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





    }
}
