using Godot;
using System;

public partial class LaserTurret : Node3D
{
    [Export]private NodePath _detectionPath;
    [Export]private NodePath _valuePath;
    [Export]private NodePath _hurtboxPath;
    [Export]private NodePath _healthPath;
    [Export]private NodePath _targetablePath;
    [Export]private NodePath _targetPath;
    [Export]private NodePath _evalPath;
    [Export]private NodePath _laserAttackPath;

    private DetectionComponent _detection;
    private ValueComponent _value;
    private HurtBoxComponent _hurtbox;
    private HealthComponent _health;
    private TargetableComponent _targetable;
    private TargetComponent _target;
    private EvaluateComponent _eval;
    private LaserAttackComponent _laserAttack;
    public override void _Ready()
    {
        _detection = GetNode<DetectionComponent>(_detectionPath);
        _value = GetNode<ValueComponent>(_valuePath);
        _hurtbox = GetNode<HurtBoxComponent>(_hurtboxPath);
        _health = GetNode<HealthComponent>(_healthPath);
        _targetable = GetNode<TargetableComponent>(_targetablePath);
        _target = GetNode<TargetComponent>(_targetPath);
        _eval = GetNode<EvaluateComponent>(_evalPath);
        _laserAttack = GetNode<LaserAttackComponent>(_laserAttackPath);

        _detection.EntryDetected += _eval.eval;
        _eval.TargetEvaluated += _target.onTargetEvaluated;
        _target.targetChanged += _laserAttack.newTargetSpottet;
    }
}
