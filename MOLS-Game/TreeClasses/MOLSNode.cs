namespace MOLS_Game.TreeClasses
{
    public class MOLSNode
    {
        public string[] Tiles { get; private set; }
        public bool IsMOLS { get; private set; }
        public string Path { get; private set; }

        public MOLSNode(string[] tiles, string path = "")
        {
            Tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
            Path = path;
            IsMOLS = TileEditor.CheckIfMOLS(tiles);
        }

        public MOLSNode Move(string direction, int steps)
        {
            string[] newTiles = TileEditor.MoveTile(Tiles, direction.Equals("Vertical"), steps);
            string newPath = $"{Path}{direction}{steps}, ";
            return new MOLSNode(newTiles, newPath);
        }
    }
}
