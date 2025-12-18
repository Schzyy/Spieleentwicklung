using Godot;
using System;

public partial class LevelTwo : Node3D
{
    private PackedScene wallScene;

    public override void _Ready()
    {
        wallScene = (PackedScene)ResourceLoader.Load("res://Scenes/Misc/wall.tscn");
    
        // PlaceWall(new Vector3(0,0,0));    
    }
    private void PlaceWall(Vector3 pos)
    {
        Node3D wall = (Node3D)wallScene.Instantiate();
        wall.Position = pos;
        AddChild(wall);
    }

}
