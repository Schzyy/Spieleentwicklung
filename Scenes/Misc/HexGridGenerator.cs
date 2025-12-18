using Godot;

public partial class HexGridGenerator : Node3D
{
    [Export] public PackedScene HexTileScene;
    [Export] public int Radius = 10;
    [Export] public float TileSize = 1.0f;

    public override void _Ready()
    {
        GenerateHexGrid();
    }

    private void GenerateHexGrid()
    {
        for (int q = -Radius; q <= Radius; q++)
        {
            for (int r = -Radius; r <= Radius; r++)
            {
                int s = -q - r;
                if (Mathf.Abs(s) <= Radius)
                {
                    var tile = HexTileScene.Instantiate<Node3D>();
                    tile.Position = HexMath.AxialToWorld(q, r, TileSize);
                    AddChild(tile);
                }
            }
        }
    }
}
