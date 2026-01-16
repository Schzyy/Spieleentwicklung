using Godot;
using System;

public partial class OrientationComponent : Node3D
{
    [Export] private Node3D _owner;
    [Export] private bool _includeY = false;
    [Export] private float _rotationSpeed;
    private Node3D _target;
    public void FaceDirection(Node3D body)
    {
        _target = body;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(_owner == null || _target == null)
        {
            return;
        }
        Vector3 lookDirection = _target.GlobalPosition;
        if(_includeY == false) 
        {
            lookDirection.Y = 0;
        }
        if (lookDirection.LengthSquared() < 0.001f)
        {
            return;
        }
        Basis targetBasis = Basis.LookingAt(lookDirection, Vector3.Up);
        Basis currentBasis = GlobalTransform.Basis;
        Basis newBasis = currentBasis.Slerp(targetBasis, _rotationSpeed * (float)delta);
        _owner.GlobalTransform = new Transform3D(newBasis, GlobalTransform.Origin);
    }
}
