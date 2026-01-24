using Godot;
using System;
using System.Collections.Generic;
public partial class LaserAttackComponent : Node3D
{
    [Export]private NodePath _muzzlePath;
    private Node3D muzzle;
    private Node3D currentTarget;
    public override void _PhysicsProcess(double delta)
    {
        muzzle = GetNode<Node3D>(_muzzlePath);
    }
    public override void _Process(double delta)
    {
        if(currentTarget == null)
        {
            return;
        }
        DebugDraw3D.DrawLine(muzzle.GlobalPosition, currentTarget.GlobalPosition, Colors.AliceBlue);
    }
    public void newTargetSpottet(Node3D body)
    {
        currentTarget = body;
    }
}
