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
    public override void _ExitTree()
    {
        TargetDestroyed?.Invoke(_owner);
    }

    public Node3D AsNode()
    {
        throw new NotImplementedException();
    }

}
