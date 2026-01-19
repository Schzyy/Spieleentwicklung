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
    private MeleeAttackComponent _attack;
    private Marker3D _mainTarget;
    public override void _Ready()
    {
        _detection = GetNode<DetectionComponent>(_detectionPath);
        _evaluate = GetNode<EvaluateComponent>(_evaluatePath);
        _value = GetNode<ValueComponent>(_valuePath);
        _target = GetNode<TargetComponent>(_targetPath);
        _path = GetNodeOrNull<PathComponent>(_pathfindPath);
        _orientation = GetNode<OrientationComponent>(_orientationPath);
        _attack = GetNode<MeleeAttackComponent>(_attackPath);
        GD.Print(GetNode<MeleeAttackComponent>(_attackPath));
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

        MeshInstance3D bearMesh = GetNode<MeshInstance3D>("Bear");
        Aabb bounds = bearMesh.GetAabb();
        GD.Print("Local bounds min: ", bounds.Position, " size: ", bounds.Size);
    }
}