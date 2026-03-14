using Godot;

public partial class Enemy : CharacterBody3D
{
    [Export] private DetectionComponent _detection;
    [Export] private TargetComponent _target;
    [Export] private PathComponent _path;
    [Export] private OrientationComponent _orientation;
    [Export] private MeleeAttackComponent _attack;
    [Export] private AnimationPlayer _animPlayer;

    public override void _Ready()
    {
        if (_path != null)
        {
            _target.SetMainTarget(_path.MainTarget);
        }

        _detection.EntryDetected += _target.SetTarget;
        _target.TargetChanged += _path.MoveTo;
        _target.TargetLost += _path.MoveToMain;
        _target.TargetChanged += _orientation.FaceTarget;
        _target.TargetLost += _orientation.ClearTarget;

        PlayIdle();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_path != null)
        {
            _orientation.FaceMovement(_path.CurrentDirection);
        }
        PlayIdle();
    }

    public void PlayAttack()
    {
        _animPlayer.Play("attack");
    }

    public void PlayDeath()
    {
        _animPlayer.Play("death");
    }

    public void PlayIdle()
    {
        _animPlayer.Play("idle");
    }

    public void OnAnimationFinished(StringName name)
    {
        if (name == "attack")
            PlayIdle();
    }
}