using Godot;
using System;

public partial class HealthComponent : Node3D, ITargetable
{
    [Export] public int MaxHealth = 100;
    [Export] private HealthBar _healthBar;

    private int _health;
    private bool _isDead = false;

    public int Health => _health;

    public event Action Died;
    public event Action<Node3D> TargetDestroyed;

    public override void _Ready()
    {
        _health = MaxHealth;
        _healthBar?.Init(this);
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;
        _health -= damage;
        _healthBar?.UpdateHealth();
        if (_health <= 0)
        {
            _isDead = true;
            Die();
        }
    }

    private void Die()
    {
        var owner = GetParent<Node3D>();
        Died?.Invoke();
        TargetDestroyed?.Invoke(owner);
        // TODO: Replace Name comparison with IsInGroup("castle") once the castle scene uses a group
        if (owner.Name == "Castle")
        {
            GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, "res://Over.tscn");
            return;
        }
        owner.QueueFree();
    }

    public Node3D AsNode() => GetParent<Node3D>();
}