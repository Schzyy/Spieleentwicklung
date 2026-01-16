using Godot;
using System;
public partial class DetectionComponent : Area3D
{
    public event Action<Node3D> EntryDetected;
    public override void _Ready()
    {
        AreaEntered += someoneNewEntered;
    }
    private void someoneNewEntered(Node3D body)
    {
        
        EntryDetected?.Invoke(body);
    }
}