using Godot;
using System;

public partial class PathComponent : Node3D
{
    public event Action<Node3D> TargetReached;

    [Export] public float StopDistance = 3f;
    [Export] public float MoveSpeed = 3f;
    [Export] private float _wanderRadius = 0.5f;
    [Export] private float _wanderUpdateTime = 0.3f;
    [Export] private float _gravity = 1000f;

    private Node3D _mainTarget;
    public Node3D MainTarget => _mainTarget;

    private NavigationAgent3D _agent;
    private CharacterBody3D _owner;
    private Node3D _target;

    public Vector3 CurrentDirection { get; private set; } = Vector3.Zero;

    private Vector3 _currentWanderOffset = Vector3.Zero;
    private double _timeSinceLastWander = 0;

    public override void _Ready()
    {
        _owner = GetParent<CharacterBody3D>();
        _agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _mainTarget = SetMainTarget();
    }

    public void MoveTo(Node3D target)
    {
        _target = target;
        if (IsInstanceValid(_target))
        {
            _agent.TargetPosition = _target.GlobalPosition;
        }
    }

    private Node3D SetMainTarget()
    {
        return GetTree().GetFirstNodeInGroup("main_target") as Node3D;
    }

    public void MoveToMain()
    {
        _target = null;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_target != null && !IsInstanceValid(_target))
            _target = null;

        if (_target == null)
        {
            _target = _mainTarget;
            if (_target == null || !IsInstanceValid(_target))
            {
                CurrentDirection = Vector3.Zero;
                return;
            }
        }

        _agent.TargetPosition = _target.GlobalPosition;

        float distanceToTarget = _owner.GlobalPosition.DistanceTo(_target.GlobalPosition);
        if (distanceToTarget <= StopDistance)
        {
            CurrentDirection = Vector3.Zero;
            return;
        }

        _timeSinceLastWander += delta;
        if (_timeSinceLastWander >= _wanderUpdateTime)
        {
            _timeSinceLastWander = 0;
            _currentWanderOffset = new Vector3(
                (float)(GD.Randf() * 2 - 1) * _wanderRadius,
                0,
                (float)(GD.Randf() * 2 - 1) * _wanderRadius
            );
        }

        Vector3 nextPos = _agent.GetNextPathPosition();
        Vector3 targetPosWithWander = nextPos + _currentWanderOffset;
        Vector3 dir = (targetPosWithWander - _owner.GlobalPosition).Normalized();

        if (!_owner.IsOnFloor())
        {
            dir.Y -= _gravity * (float)delta;
        }

        CurrentDirection = dir;
        _owner.Velocity = dir * MoveSpeed;
        _owner.MoveAndSlide();
    }
}