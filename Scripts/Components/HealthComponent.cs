using Godot;
using System;

public partial class HealthComponent : Node3D
{
    [Export] public int Max_health = 100;
    [Export] public HealthBar hBar;
    public int health;
    private bool _isDead = false;

    public override void _Ready()
    {
        hBar.init(this);

        health = Max_health;
    }
    public void takeDamage(int damage)
    {
        health = health - damage;
        if(health <= 0 && GetParent().Name == "Castle")
        {
            GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, "res://Over.tscn");
        }
        if(health <= 0)
        {
            _isDead = true;
            Die();
        }
        hBar.updateHealth();
    }
    private void Die()
    {
        GetParent().QueueFree();
    }
}
