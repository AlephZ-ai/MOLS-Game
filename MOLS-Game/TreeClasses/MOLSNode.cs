namespace MOLS_Game.TreeClasses
{
    public class MOLSNode
{
        private string[] tiles;

        private MOLSNode? down = null;
        private MOLSNode? up = null;
        private MOLSNode? left = null;
        private MOLSNode? right = null;
        private MOLSNode? parent = null;
        private string path = "";
        private int priority = 0;

        public MOLSNode(string[] tiles)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            this.tiles = tiles;
        }

        public MOLSNode(string[] tiles, string step, MOLSNode parent)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            this.tiles = tiles;
            path = step;
            this.parent = parent;
        }
        public MOLSNode(string[] tiles, string step, MOLSNode parent, int priority)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            this.tiles = tiles;
            path = step;
            this.parent = parent;
            this.priority = priority;
        }

        public int GetPriority()
        {
            return priority;
        }

        public string GetOverallPath()
        {
            if(parent == null)
            {
                return "";
            }
            

            return parent.GetOverallPath() + path;

            
        }

       public string GetPath()
        {
            return path;
        }

        public string[] GetTiles()
        {
            return tiles;
        }

        public MOLSNode? GetDown()
        {
            return down;
        }

        public MOLSNode? GetUp()
        {
            return up;
        }

        public MOLSNode? GetLeft()
        {
            return left;
        }

        public MOLSNode? GetRight() 
        {
            return right;
        }

        public MOLSNode? GetParent()
        {
            return parent;
        }

        public void SetDown(MOLSNode? down)
        {
            this.down = down;
        }

        public void SetUp(MOLSNode? up) 
        { 
            this.up = up;
        }

        public void SetRight(MOLSNode? right)
        {
            this.right = right;
        }

        public void SetLeft(MOLSNode? left)
        {
            this.left = left;
        }

        public void SetParent(MOLSNode? parent)
        {
            this.parent = parent;
        }
}
}
