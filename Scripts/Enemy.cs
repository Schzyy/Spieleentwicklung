using Godot;

public partial class Enemy : CharacterBody3D
{
    [Export] private DetectionComponent _detection;
    [Export] private EvaluateComponent _evaluate;
    [Export] private ValueComponent _value;
    [Export] private TargetComponent _target;
    [Export] private PathComponent _path;
    [Export] private OrientationComponent _orientation;
    [Export] private AttackComponent _attack;

    public override void _Ready()
    {
        GD.Print(_detection, _evaluate, _value, _target, _path, _orientation,_attack);
        _detection.EntryDetected += _evaluate.eval;
        _evaluate.TargetEvaluated += _target.onTargetEvaluated;
        if(_attack != null)
        {   
        }
        if(_path != null)
        {
        }
        if(_orientation != null)
        {
            _target.targetChanged += _orientation.FaceDirection;
        }
    }
}