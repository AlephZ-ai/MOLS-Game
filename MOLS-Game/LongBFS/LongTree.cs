using MOLS_Game.TreeClasses;

namespace MOLS_Game.LongBFS
{
    public class LongTree
{

        private LongNode root;

        public LongTree(string[] tiles, int emptyIndex)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            root = new LongNode(tiles, emptyIndex,true);
        }

        public LongNode GetRoot() { return root; }

      


    }
}
