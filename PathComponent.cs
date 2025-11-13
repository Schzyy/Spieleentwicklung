using Godot;
using System;

public partial class PathComponent : Node
{
    public event Action<Node3D> TargetReached;

    [Export] private float stopDistance = 1.5f;
    [Export] private float moveSpeed = 3f;

    private NavigationAgent3D _agent;
    private CharacterBody3D _owner;
    private Node3D _mainTarget;
    private Node3D _target;

    public override void _Ready()
    {
        _owner = GetParent<CharacterBody3D>();
        _agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _mainTarget = null;
        // Optional: Callback wenn Ziel erreicht
        _agent.NavigationFinished += OnNavigationFinished;
    }
    public void setMainTarget(Node3D target)
    {
        _mainTarget = target;
    }
    public void MoveTo(Node3D target)  
    {
        _target = target;
        _agent.TargetPosition = target.GlobalPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_target == null)
        {
            _target = _mainTarget;
        }
        if(_target == null && _mainTarget == null)
        {
            return;
        }
        // Aktualisiere Zielposition, falls das Ziel sich bewegt (z. B. Spieler)
        _agent.TargetPosition = _target.GlobalPosition;

        if (_agent.IsNavigationFinished())
        {
            TargetReached?.Invoke(_target);
            return;
        }

        // Bewegung Richtung nächsten Pfadpunkt
        Vector3 nextPos = _agent.GetNextPathPosition();
        Vector3 dir = (nextPos - _owner.GlobalPosition).Normalized();
        _owner.Velocity = dir * moveSpeed;
        _owner.MoveAndSlide();
    }

    private void OnNavigationFinished()
    {
        if (_target != null)
            TargetReached?.Invoke(_target);
    }
}
