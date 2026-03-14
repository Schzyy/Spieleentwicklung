using Godot;
using System;
public partial class TargetableComponent : Node3D, ITargetable
{
    public event Action<Node3D> TargetDestroyed;

    private Node3D _owner;

    public override void _Ready()
    {
        _owner = GetParent<Node3D>();
    }

    public void Die()
    {
        GD.Print("Targetable: Die() called");

        TargetDestroyed?.Invoke(_owner);

        _owner.QueueFree();
    }

    public Node3D AsNode()
    {
        return _owner;
    }
}
