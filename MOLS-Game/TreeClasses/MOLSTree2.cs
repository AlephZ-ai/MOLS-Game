namespace MOLS_Game.TreeClasses
{
    public class MOLSTree2
{

        private MOLSNode2 root;

        
        public MOLSTree2(string[] tiles, string HPath)
        {
            root = new MOLSNode2(tiles, HPath);
        }
        public MOLSNode2 GetRoot() { return root; }
    }
}
