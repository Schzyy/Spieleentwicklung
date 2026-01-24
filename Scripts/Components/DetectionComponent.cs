using Godot;
using System;
public partial class DetectionComponent : Area3D
{
    public event Action<Node3D> EntryDetected;
    public override void _Ready()
    {
        AreaEntered += someoneNewEntered;
        BodyEntered += someoneNewEntered;
    }
    private void someoneNewEntered(Node3D body)
    {
        GD.Print(body.GetParent().Name);
        EntryDetected?.Invoke(body);
    }
}