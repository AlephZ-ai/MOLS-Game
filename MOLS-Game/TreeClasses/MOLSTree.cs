namespace MOLS_Game.TreeClasses
{
    public class MOLSTree
{


        private MOLSNode root;
        public MOLSTree(string[] tiles)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            Console.WriteLine(tiles.Length);
            root = new MOLSNode(tiles);
        }

        public MOLSNode GetRoot() { return root; }

        //define a function that gets the steps from one to another.

}
}
