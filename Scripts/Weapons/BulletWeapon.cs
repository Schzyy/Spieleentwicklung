using Godot;

/// <summary>
/// Bullet-based weapon. Fires projectiles from a muzzle point using a bullet pool.
/// </summary>
public partial class BulletWeapon : WeaponComponent
{
    [Export] private NodePath _bulletPoolPath;
    [Export] private Node3D _muzzle;

    private BulletPoolComponent _bulletPool;

    public override void _Ready()
    {
        if (_bulletPoolPath != null)
            _bulletPool = GetNode<BulletPoolComponent>(_bulletPoolPath);
    }

    public override void TryAttack(Node3D target)
    {
        if (_cooldownTimer > 0 || _bulletPool == null || _muzzle == null)
            return;

        var bullet = _bulletPool.GetBullet();
        if (bullet == null)
            return;

        Vector3 dir = (target.GlobalPosition - _muzzle.GlobalPosition).Normalized();
        bullet.GlobalPosition = _muzzle.GlobalPosition;
        bullet.Init(dir);

        _cooldownTimer = AttackCooldown;
    }
}
