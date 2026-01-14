using Godot;
using System;

public partial class HurtBoxComponent : Area3D
{
    [Export] public HealthComponent healthComponent;
    public void OnHit(Bullet bullet)
    {
        healthComponent.takeDamage(bullet.Damage);
    }
}
