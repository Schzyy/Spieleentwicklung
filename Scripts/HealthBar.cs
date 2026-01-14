using Godot;
using System;

public partial class HealthBar : Node3D
{
    [Export]public ProgressBar pBar;
    public HealthComponent healthComp;

    public void init(HealthComponent hComp)
    {
        healthComp = hComp;
        pBar.MaxValue = healthComp.Max_health;
        pBar.Value = healthComp.Max_health;
    }

    public void updateHealth()
    {
        pBar.Value = healthComp.health;
    }
}
