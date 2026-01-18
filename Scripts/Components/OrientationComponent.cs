using Godot;
using System;
public partial class OrientationComponent : Node3D
{
    [Export] private bool _includeY = false;
    [Export] private float _rotationSpeed = 5f;

    private Node3D _owner;
    private Node3D _target;

    public override void _Ready()
    {
        _owner = GetParent<Node3D>();
    }

    public void FaceDirection(Node3D target)
    {
        _target = target;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_owner == null || _target == null)
            return;

        Vector3 lookDirection =
            _target.GlobalPosition - _owner.GlobalPosition;

        if (!_includeY)
            lookDirection.Y = 0;

        if (lookDirection.LengthSquared() < 0.0001f)
            return;

        lookDirection = lookDirection.Normalized();

        Basis targetBasis = Basis.LookingAt(lookDirection, Vector3.Up);
        Basis currentBasis = _owner.GlobalTransform.Basis;

        Basis newBasis = currentBasis.Slerp(
            targetBasis,
            _rotationSpeed * (float)delta
        );

        _owner.GlobalTransform = new Transform3D(
            newBasis,
            _owner.GlobalTransform.Origin
        );
    }
}
