using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    private NavigationAgent3D navAgent;

    [Export] public float Speed = 4f;
    [Export] public Node3D target;

    public override void _Ready()
    {
        AddToGroup("Enemy");
        navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        if(target != null)
        {
            navAgent.TargetPosition = target.GlobalTransform.Origin;
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        if (navAgent.IsNavigationFinished())
        {
            return;
        }
        Vector3 nextPathPos = navAgent.GetNextPathPosition();
        Vector3 direction = (nextPathPos - GlobalTransform.Origin).Normalized();
        Velocity = direction * Speed;
        MoveAndSlide();
    }   
    public void setTarget(Node3D target)
    {
        this.target = target;
        navAgent.TargetPosition = target.GlobalTransform.Origin;
    }

}
