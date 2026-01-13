using Godot;
using System;

public partial class PathComponent : Node3D
{
    public event Action<Node3D> TargetReached;

    [Export] private float stopDistance = 1.5f;
    [Export] private float moveSpeed = 3f;
    [Export] private float wanderRadius = 0.5f;   // Random deviation from the path
    [Export] private float wanderUpdateTime = 0.3f; // How often to pick a new random offset

    private NavigationAgent3D _agent;
    private CharacterBody3D _owner;
    private Node3D _mainTarget;
    public Node3D _target;

    private Vector3 _currentWanderOffset = Vector3.Zero;
    private double _timeSinceLastWander = 0;

    public override void _Ready()
    {
        _owner = GetParent<CharacterBody3D>();
        _agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _mainTarget = null;
    }

    public void SetMainTarget(Node3D target)
    {
        _mainTarget = target;
    }

    public void MoveTo(Node3D target)
    {
        _target = target;
        if (IsInstanceValid(_target))
            _agent.TargetPosition = _target.GlobalPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_target != null && !IsInstanceValid(_target))
            _target = null;

        if (_target == null)
        {
            _target = _mainTarget;
            if (_target == null || !IsInstanceValid(_target))
                return;
        }

        // UPDATE PATH TARGET
        _agent.TargetPosition = _target.GlobalPosition;

        // CHECK DISTANCE
        float distanceToTarget = _owner.GlobalPosition.DistanceTo(_target.GlobalPosition);
        if (distanceToTarget <= stopDistance || _agent.IsNavigationFinished())
        {
            HandleTargetReached();
            return;
        }

        // RANDOMIZED WANDERING LOGIC
        _timeSinceLastWander += delta;
        if (_timeSinceLastWander >= wanderUpdateTime)
        {
            _timeSinceLastWander = 0;
            _currentWanderOffset = new Vector3(
                (float)(GD.Randf() * 2 - 1) * wanderRadius,
                0,
                (float)(GD.Randf() * 2 - 1) * wanderRadius
            );
        }

        // CALCULATE MOVEMENT
        Vector3 nextPos = _agent.GetNextPathPosition();
        Vector3 targetPosWithWander = nextPos + _currentWanderOffset;
        Vector3 dir = (targetPosWithWander - _owner.GlobalPosition).Normalized();

        _owner.Velocity = dir * moveSpeed;
        _owner.MoveAndSlide();
    }

    public void HandleTargetReached()
    {
        Node3D reachedTarget = _target;

        if (_target != _mainTarget)
            _target = null;

        TargetReached?.Invoke(reachedTarget);
    }
}
