namespace MOLS_Game.TreeClasses
{
    public class MOLSNode
{
        private string[] tiles = new string[16];
        
        private MOLSNode? down = null;
        private MOLSNode? up = null;
        private MOLSNode? left = null;
        private MOLSNode? right = null;
        private MOLSNode? parent = null;


        public MOLSNode(string[] tiles)
            {
                this.tiles=tiles;
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
