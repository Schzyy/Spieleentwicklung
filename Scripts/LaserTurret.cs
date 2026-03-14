using Godot;

public partial class LaserTurret : Node3D
{
    [Export] private DetectionComponent _detection;
    [Export] private TargetComponent _target;
    [Export] private LaserWeapon _laserWeapon;

    public override void _Ready()
    {
        _detection.EntryDetected += _target.SetTarget;
        _target.TargetChanged += _laserWeapon.SetTarget;
    }
}
