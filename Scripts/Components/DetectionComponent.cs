using Godot;
using System;

public partial class DetectionComponent : Area3D
{
    public event Action<Node3D> EntryDetected;
    public event Action<Node3D> ExitDetected;

    public override void _Ready()
    {
        AreaEntered += OnEntryDetected;
        BodyEntered += OnEntryDetected;
        AreaExited += OnExitDetected;
        BodyExited += OnExitDetected;
    }

    private void OnEntryDetected(Node3D body)
    {
        GD.Print(body.Name);
        EntryDetected?.Invoke(body);
    }

    private void OnExitDetected(Node3D body)
    {
        ExitDetected?.Invoke(body);
    }
}