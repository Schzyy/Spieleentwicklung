using Godot;

public partial class HealthBar : Node3D
{
    [Export] private Node3D enemy; // The enemy node
    [Export] private MeshInstance3D foreground; // The green/red bar
    [Export] private MeshInstance3D background; // The full bar
    [Export] private Vector3 offset = new Vector3(0, 2, 0); // Bar above enemy

    private HealthComponent _healthComponent;
    private int _maxHealth;

    public override void _Ready()
    {
        if (enemy == null) 
            enemy = GetParent<Node3D>();

        _healthComponent = enemy.GetNode<HealthComponent>("HealthComponent");

        if (_healthComponent != null)
            _maxHealth = _healthComponent.Max_health; // Use public property

        // Optional: face the camera immediately
        LookAt(GetViewport().GetCamera3D().GlobalPosition, Vector3.Up);
    }

    public override void _Process(double delta)
    {
        if (_healthComponent == null) return;

        // Position above enemy
        GlobalPosition = enemy.GlobalPosition + offset;

        // Face the camera
        LookAt(GetViewport().GetCamera3D().GlobalPosition, Vector3.Up);

        // Update foreground scale
        int currentHealth = _healthComponent.health; // Use public getter
        float ratio = Mathf.Clamp((float)currentHealth / _maxHealth, 0, 1);

        if (foreground != null)
        {
            var scale = foreground.Scale;
            scale.X = ratio;
            foreground.Scale = scale;
        }
    }
}
