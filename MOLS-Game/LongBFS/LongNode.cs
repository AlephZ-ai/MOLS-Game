using MOLS_Game.TreeClasses;
using System.IO;

namespace MOLS_Game.LongBFS
{
    public class LongNode
{

        private string[] tiles;

        private LongNode? parent = null;
        private int emptyIndex;
        private string? step;
        bool vertical;

        public LongNode(string[] tiles, int emptyIndex, bool vertical)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            this.tiles = tiles;
            this.emptyIndex = emptyIndex;
            this.vertical = vertical;
        }

        public LongNode(string[] tiles, LongNode parent, int emptyIndex,string step,bool vertical)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            this.tiles = tiles;
            this.step = step;
            this.parent = parent;
            this.emptyIndex = emptyIndex;
            this.vertical = vertical;
        }
      

        public bool IsVertical { get { return vertical; } }

        public int GetEmptyIndex() { return emptyIndex; }

        public string GetOverallPath()
        {
            if (parent == null)
            {
                return "";
            }


            return parent.GetOverallPath() + step;


        }

       


        public string[] GetTiles()
        {
            return tiles;
        }

       
       

        public LongNode? GetParent()
        {
            return parent;
        }



    }
}
