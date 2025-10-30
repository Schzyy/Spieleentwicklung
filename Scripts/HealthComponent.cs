using Godot;
using System;

public partial class HealthComponent : Node3D
{
    [Export] private int Max_health = 100;
    private int health;

    public override void _Ready()
    {
        health = Max_health;
    }
    public void takeDamage(int damage)
    {
        health = health - damage;
    }
}
