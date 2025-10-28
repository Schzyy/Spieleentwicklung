using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    private NavigationAgent3D navigationAgent3D;

    [Export]
    public float Speed = 4f;

    public override void _Ready()
    {
        navigationAgent3D = GetNode<NavigationAgent3D>("NavigationAgent3D");
    }
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("ui_home"))
        {
            Vector3 targetPos = new Vector3
            {
                X = (float)GD.RandRange(-20.0, 20.0),
                Z = (float)GD.RandRange(-20.0, 20.0)
            };
            navigationAgent3D.TargetPosition = targetPos;
        }
        Vector3 nextWaypoint = navigationAgent3D.GetNextPathPosition();
        Vector3 vectorToNextWaypoint = nextWaypoint - GlobalPosition;

        Vector3 direction = vectorToNextWaypoint.Normalized();
        Velocity = direction * Speed;
        MoveAndSlide();
    }

}
