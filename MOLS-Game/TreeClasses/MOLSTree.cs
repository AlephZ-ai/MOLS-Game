using MOLS_Game.TreeClasses;

public class MOLSTree
{
    private MOLSNode root;

    public MOLSTree(string[] tiles)
    {
        if (tiles == null) throw new ArgumentNullException(nameof(tiles));
        root = new MOLSNode(tiles);
    }

    public MOLSNode GetRoot() { return root; }

    public void PruneTree()
    {
        PruneNode(root);
        root = null;
    }

    private void PruneNode(MOLSNode node)
    {
        if (node == null)
            return;

        PruneNode(node.GetDown());
        PruneNode(node.GetUp());
        PruneNode(node.GetLeft());
        PruneNode(node.GetRight());

        DetachNode(node);
    }

    private void DetachNode(MOLSNode node)
    {
        MOLSNode? parent = node.GetParent();
        if (parent != null)
        {
            if (parent.GetDown() == node)
                parent.SetDown(null);
            else if (parent.GetUp() == node)
                parent.SetUp(null);
            else if (parent.GetLeft() == node)
                parent.SetLeft(null);
            else if (parent.GetRight() == node)
                parent.SetRight(null);
        }

        node.SetDown(null);
        node.SetUp(null);
        node.SetLeft(null);
        node.SetRight(null);
        node.SetParent(null);
    }
}