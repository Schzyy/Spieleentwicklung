using Godot;
using System;

public partial class HealthBar : Node3D
{
    [Export] public ProgressBar pBar;
    public HealthComponent HealthComp;

    public void Init(HealthComponent hComp)
    {
        HealthComp = hComp;
        pBar.MaxValue = HealthComp.MaxHealth;
        pBar.Value = HealthComp.MaxHealth;
    }

    public void UpdateHealth()
    {
        pBar.Value = HealthComp.Health;
    }
}
