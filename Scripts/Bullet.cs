using Godot;

public partial class Bullet : Area3D
{
    [Export] public float Speed = 20f;
    [Export] public float LifeTime = 3f;

    private Vector3 _direction;
    private float _lifeTimer;
    private bool _active;

    public void Init(Vector3 direction)
    {
        _direction = direction.Normalized();
        _lifeTimer = LifeTime;
        _active = true;
        Visible = true;
        SetProcess(true);
        SetPhysicsProcess(true);
        Monitoring = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_active) return;

        GlobalPosition += _direction * Speed * (float)delta;
        _lifeTimer -= (float)delta;

        if (_lifeTimer <= 0)
            Deactivate();
    }

    private void Deactivate()
    {
        _active = false;
        Visible = false;
        Monitoring = false;
        SetPhysicsProcess(false);
        SetProcess(false);
    }

    public bool IsActive => _active;

    private void _on_body_entered(Node3D body)
    {
        if (!_active) return;
        GD.Print($"Bullet hit {body.Name}");
        Deactivate();
    }
}
