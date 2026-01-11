using Godot;
using System;

public partial class HurtBoxComponent : Area3D
{
    [Export] public string[] AllowedGroups { get; set; } = new string[0];
    [Export] public HealthComponent healthComponent;
    [Export] public NavigationRegion3D navReg;
    public event Action confirmDeath;
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        AreaEntered += OnAreaEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Damage"))
        {
            healthComponent.takeDamage(10);
        }
    }
    private void OnAreaEntered(Area3D area)
    {
        {
            healthComponent.takeDamage(10);
        }
    }
    private void cooked()
    {
    }
}
