using Godot;

public partial class Enemy : CharacterBody3D
{
    [Export] private NodePath _detectionPath;
    [Export] private NodePath _evaluatePath;
    [Export] private NodePath _valuePath;
    [Export] private NodePath _targetPath;
    [Export] private NodePath _pathfindPath;
    [Export] private NodePath _orientationPath;
    [Export] private NodePath _attackPath;
    private DetectionComponent _detection;
    private EvaluateComponent _evaluate;
    private ValueComponent _value;
    private TargetComponent _target;
    private PathComponent _path;
    private OrientationComponent _orientation;
    private AttackComponent _attack;
    private Marker3D _mainTarget;
    public override void _Ready()
    {
        _detection = GetNode<DetectionComponent>("DetectionComponent");
        _evaluate = GetNode<EvaluateComponent>("EvaluateComponent");
        _value = GetNode<ValueComponent>("ValueComponent");
        _target = GetNode<TargetComponent>("TargetComponent");
        _path = GetNodeOrNull<PathComponent>("PathComponent");
        _orientation = GetNode<OrientationComponent>("OrientationComponent");
        _attack = GetNodeOrNull<AttackComponent>("AttackComponent");
        GD.Print(_detection, _evaluate, _value, _target, _path, _orientation,_attack);
        if(_evaluate != null && _value != null)
        {
            _evaluate.attachValue(_value);
        }
        if(_attack != null)
        {
            
        }
        if(_path != null)
        {
            _target.mainPOI(_path.mainTarget);            
        }
        if(_orientation != null)
        {
            _target.targetChanged += _orientation.FaceDirection;
        }
        _detection.EntryDetected += _evaluate.eval;
        _evaluate.TargetEvaluated += _target.onTargetEvaluated;
        _target.targetChanged += _path.MoveTo;
    }
}