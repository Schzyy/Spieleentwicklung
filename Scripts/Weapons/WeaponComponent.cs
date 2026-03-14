using Godot;

/// <summary>
/// Abstract base class for all weapon components.
/// Subclasses implement TryAttack to fire at a given target.
/// </summary>
public abstract partial class WeaponComponent : Node
{
    [Export] public float AttackCooldown = 1.0f;

    protected double _cooldownTimer = 0;

    public override void _Process(double delta)
    {
        if (_cooldownTimer > 0)
            _cooldownTimer -= delta;
    }

    public abstract void TryAttack(Node3D target);
}
