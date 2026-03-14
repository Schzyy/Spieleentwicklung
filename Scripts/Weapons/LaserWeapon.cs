using Godot;

/// <summary>
/// Laser weapon. Draws a debug line to the current target and deals damage on a cooldown.
/// Replaces the incomplete LaserAttackComponent.
/// </summary>
public partial class LaserWeapon : WeaponComponent
{
    [Export] private Node3D _muzzle;
    [Export] private int _damage = 10;

    private Node3D _currentTarget;

    public void SetTarget(Node3D target)
    {
        _currentTarget = target;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_currentTarget == null || !GodotObject.IsInstanceValid(_currentTarget))
        {
            _currentTarget = null;
            return;
        }

        Vector3 origin = _muzzle != null ? _muzzle.GlobalPosition : GlobalPosition;
        DebugDraw3D.DrawLine(origin, _currentTarget.GlobalPosition, Colors.Red);

        TryAttack(_currentTarget);
    }

    public override void TryAttack(Node3D target)
    {
        if (_cooldownTimer > 0 || target == null)
            return;

        foreach (Node child in target.GetChildren())
        {
            if (child is HealthComponent health)
            {
                health.TakeDamage(_damage);
                break;
            }
        }

        _cooldownTimer = AttackCooldown;
    }
}
