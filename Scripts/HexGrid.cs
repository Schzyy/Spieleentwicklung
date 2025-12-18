using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class HexGrid : Node3D
{
    private static readonly PackedScene HexTile = ResourceLoader.Load<PackedScene>("res://Scenes/Misc/Hexagon.tscn");
    [Export] float Tile_size = 1.0f;
    [Export] [Range(2, 20)] public int Grid_size { get; set; } = 10;
    public override void _Ready()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
{
    int tileIndex = 0;

    for (int x = 0; x < Grid_size; x++)
    {
        Vector2 tileCoordinates = Vector2.Zero;
        tileCoordinates.X = x * Tile_size * Mathf.Cos(Mathf.DegToRad(30));
        tileCoordinates.Y = (x % 2 == 0) ? 0 : Tile_size / 2;

        for (int y = 0; y < Grid_size; y++)
        {
            var tile = HexTile.Instantiate<Node3D>();
            AddChild(tile);
            tile.Position = new Vector3(tileCoordinates.X, 0, tileCoordinates.Y);

            tileCoordinates.Y += Tile_size;

            tileIndex++;
        }
    }
    }
}
