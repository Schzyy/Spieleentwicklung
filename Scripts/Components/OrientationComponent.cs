using Godot;
using System;
public partial class OrientationComponent : Node3D
{
    [Export] private bool _includeY = false;
    [Export] private float _rotationSpeed = 6f;

    private Node3D _owner;
    private Node3D _target;
    private Vector3 _moveDirection = Vector3.Zero;

    public override void _Ready()
    {
        _owner = GetParent<Node3D>();
    }

    public void FaceTarget(Node3D target)
    {
        _target = target;
    }

    public void ClearTarget()
    {
        _target = null;
    }

    public void FaceMovement(Vector3 direction)
    {
        if (direction.LengthSquared() < 0.001f)
            return;

        _moveDirection = direction;
    }
public override void _PhysicsProcess(double delta)
{
    if (!GodotObject.IsInstanceValid(_owner))
        return;

    Vector3 lookDir = Vector3.Zero;

    if (GodotObject.IsInstanceValid(_target))
    {
        lookDir = _target.GlobalPosition - _owner.GlobalPosition;
    }
    else if (_moveDirection.LengthSquared() > 0.0001f)
    {
        lookDir = _moveDirection;
    }
    else
    {
        return; // ← THIS prevents the crash
    }

    if (!_includeY)
        lookDir.Y = 0;

    if (lookDir.LengthSquared() < 0.0001f)
        return; // ← ABSOLUTELY REQUIRED

    lookDir = lookDir.Normalized();

    Basis targetBasis = Basis.LookingAt(-lookDir, Vector3.Up);
    Basis current = _owner.GlobalTransform.Basis;

    _owner.GlobalTransform = new Transform3D(
        current.Slerp(targetBasis, _rotationSpeed * (float)delta),
        _owner.GlobalTransform.Origin
    );
}
}
