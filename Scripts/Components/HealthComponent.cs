using Godot;
using System;

public partial class HealthComponent : Node3D
{
    [Export] public int Max_health = 100;
    public int health;
    [Export] public bool isObstacle = false;

    public override void _Ready()
    {
        health = Max_health;
    }
    public void takeDamage(int damage)
    {

        health = health - damage;
        GD.Print("health " + health);
        if(health <= 0 && GetParent().Name == "Castle")
        {
            GetTree().CallDeferred(
                SceneTree.MethodName.ChangeSceneToFile,
                "res://Over.tscn"
            );
        }
        if(health <= 0)
        {
            GD.Print(GetParent().Name);
            GetParent().QueueFree();
        }

    }
}
