using Godot;

public partial class HitBoxComponent : Area3D
{
    [Export] private HealthComponent _healthComponent;

    public void OnHit(Node source)
    {
        if (source is not IHitSource hit)
            return;
        _healthComponent?.TakeDamage(hit.Damage);
    }
}
