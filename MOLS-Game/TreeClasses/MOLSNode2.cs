namespace MOLS_Game.TreeClasses
{
    public class MOLSNode2
    {
        private string[] tiles;

        private MOLSNode2? down = null;
        private MOLSNode2? up = null;
        private MOLSNode2? left = null;
        private MOLSNode2? right = null;
        private MOLSNode2? parent = null;
        private string path = "";
        public string? HPath = null;
        

        public MOLSNode2(string[] tiles)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            this.tiles = tiles;
        }
        public MOLSNode2(string[] tiles, string HPath)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            this.tiles = tiles;
            this.HPath = HPath;
        }

        public MOLSNode2(string[] tiles, string step, MOLSNode2 parent)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            this.tiles = tiles;
            path = step;
            this.parent = parent;
        }
        public MOLSNode2(string[] tiles, string step, MOLSNode2 parent, string HPath)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            this.tiles = tiles;
            path = step;
            this.parent = parent;
            this.HPath = HPath;
        }


        public string GetOverallPath()
        {
            if (parent == null)
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

        public MOLSNode2? GetDown()
        {
            return down;
        }

        public MOLSNode2? GetUp()
        {
            return up;
        }

        public MOLSNode2? GetLeft()
        {
            return left;
        }

        public MOLSNode2? GetRight()
        {
            return right;
        }

        public MOLSNode2? GetParent()
        {
            return parent;
        }

        public void SetDown(MOLSNode2? down)
        {
            this.down = down;
        }

        public void SetUp(MOLSNode2? up)
        {
            this.up = up;
        }

        public void SetRight(MOLSNode2? right)
        {
            this.right = right;
        }

        public void SetLeft(MOLSNode2? left)
        {
            this.left = left;
        }

        public void SetParent(MOLSNode2? parent)
        {
            this.parent = parent;
        }
    }
}
