using Godot;

public partial class Bullet : Area3D
{
    [Export] public float Speed = 20f;
    [Export] public float LifeTime = 1f;
    [Export] public int Damage = 20;

    private Vector3 _direction;
    private float _lifeTimer;
    private bool _active;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
        Monitoring = false;
    }

    public void Init(Vector3 direction)
    {
        _direction = direction.Normalized();
        _lifeTimer = LifeTime;
        _active = true;

        Visible = true;
        Monitoring = true;
        SetPhysicsProcess(true);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_active)
        {
            return;
        }

        GlobalPosition += _direction * Speed * (float)delta;

        _lifeTimer -= (float)delta;
        if (_lifeTimer <= 0f)
        {
            Deactivate();
        }
    }

    private void OnAreaEntered(Area3D area)
{
    if (!_active)
        return;

    if (area.HasMethod("OnHit"))
    {
        GD.Print("happens");
        area.CallDeferred("OnHit", this);
        Deactivate();
    }
}

    private void Deactivate()
    {
        _active = false;
        Visible = false;
        SetDeferred("monitoring", false);
        SetPhysicsProcess(false);
    }

    public bool IsActive => _active;
}