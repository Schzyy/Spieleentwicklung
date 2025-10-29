using Godot;
using System;

public partial class Turret : Node3D
{
    [Export] public float range = 10f;
    private Vector3 position;
    public override void _Ready()
    {
        position = GlobalTransform.Origin;
    }
    public override void _Process(double delta)
    {

    }
    private void aggro()
    {

    }
    private void shoot()
    {
        
    }
}
