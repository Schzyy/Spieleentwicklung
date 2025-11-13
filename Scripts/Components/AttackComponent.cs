using Godot;

public partial class AttackComponent : Node
{
    [Export] private NodePath bulletPoolPath;
    [Export] private Node3D muzzle;
    [Export] private float attackCooldown = 1.0f;

    private BulletPoolComponent _bulletPool;
    private double _cooldownTimer = 0;

    public override void _Ready()
    {
        if (bulletPoolPath != null)
            _bulletPool = GetNode<BulletPoolComponent>(bulletPoolPath);
    }

    public override void _Process(double delta)
    {
        if (_cooldownTimer > 0)
            _cooldownTimer -= delta;
    }

    public void TryAttack(Node3D target)
    {
        if (_cooldownTimer > 0 || _bulletPool == null || muzzle == null)
            return;

        var bullet = _bulletPool.GetBullet();
        if (bullet == null)
            return; 

        Vector3 dir = (target.GlobalPosition - muzzle.GlobalPosition).Normalized();

        bullet.GlobalPosition = muzzle.GlobalPosition;
        bullet.Init(dir);

        _cooldownTimer = attackCooldown;
        GD.Print("Bullet fired from pool!");
    }
}
