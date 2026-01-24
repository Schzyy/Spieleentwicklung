using Godot;
using System;

public partial class HurtBoxComponent : Area3D
{
    [Export] public HealthComponent healthComponent;

    public void OnHit(Node source)
    {
        if (source is not IHitSource hit)
            return;


        healthComponent.takeDamage(hit.Damage);
    }
}