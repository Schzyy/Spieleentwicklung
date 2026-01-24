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
    [Export] private NodePath _animPlayerPath;
    private DetectionComponent _detection;
    private EvaluateComponent _evaluate;
    private ValueComponent _value;
    private TargetComponent _target;
    private PathComponent _path;
    private OrientationComponent _orientation;
    private MeleeAttackComponent _attack;
    private AnimationPlayer _animPlayer;
    private Marker3D _mainTarget;
    public override void _Ready()
    {
        _animPlayer = GetNodeOrNull<AnimationPlayer>(_animPlayerPath);
        _detection = GetNode<DetectionComponent>(_detectionPath);
        _evaluate = GetNode<EvaluateComponent>(_evaluatePath);
        _value = GetNode<ValueComponent>(_valuePath);
        _target = GetNode<TargetComponent>(_targetPath);
        _path = GetNodeOrNull<PathComponent>(_pathfindPath);
        _orientation = GetNode<OrientationComponent>(_orientationPath);
        _attack = GetNode<MeleeAttackComponent>(_attackPath);
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
        }
        _detection.EntryDetected += _evaluate.eval;
        _evaluate.TargetEvaluated += _target.onTargetEvaluated;
        _target.targetChanged += _path.MoveTo;
        _target.targetDied += _path.MoveToMain;

        MeshInstance3D bearMesh = GetNode<MeshInstance3D>("Bear");
        Aabb bounds = bearMesh.GetAabb();
    }   
    public void playAttack()
    {
        _animPlayer.Play("attack");
    }
}