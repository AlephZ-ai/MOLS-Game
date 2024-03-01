namespace MOLS_Game.TreeClasses
{
    public class MOLSTree
{


        private MOLSNode root;
        public MOLSTree(string[] tiles)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            root = new MOLSNode(tiles);
        }

        public MOLSNode GetRoot() { return root; }


}
}
