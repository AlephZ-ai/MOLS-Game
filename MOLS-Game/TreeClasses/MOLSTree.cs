namespace MOLS_Game.TreeClasses
{
    public class MOLSTree
{


        private MOLSNode root;
        public MOLSTree(string[] tiles)
        {
            root = new MOLSNode(tiles);
        }

        public MOLSNode GetRoot() { return root; }

        //define a function that gets the steps from one to another.

}
}
