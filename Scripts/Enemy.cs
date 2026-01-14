using Godot;

public partial class Enemy : CharacterBody3D
{
    [Export] private float rotationSpeed = 10f;
    private DetectionComponent _detection;
    private PathComponent _path;
    private AttackComponent _attack;
    private HealthComponent _health;
    private Node3D goal;
    
    public override void _Ready()
    {
        _detection = GetNode<DetectionComponent>("DetectionComponent");
        _path = GetNode<PathComponent>("PathComponent");
        _attack = GetNode<AttackComponent>("AttackComponent");
        _health = GetNode<HealthComponent>("HealthComponent");
        
        _detection.targetDetected += OnTargetDetected;
        _path.TargetReached += OnTargetReached;
    }
    
    public void FaceDirection(Vector3 direction, double delta)
{
    Vector3 lookDirection = direction;
    lookDirection.Y = 0;
    
    if (lookDirection.LengthSquared() < 0.001f)
        return;
    
    Basis targetBasis = Basis.LookingAt(lookDirection, Vector3.Up);
    Basis currentBasis = GlobalTransform.Basis;
    Basis newBasis = currentBasis.Slerp(targetBasis, rotationSpeed * (float)delta);
    GlobalTransform = new Transform3D(newBasis, GlobalTransform.Origin);
}
public override void _Process(double delta)
{
    if (goal == null)
        return;
    
    if (_path.CurrentDirection.LengthSquared() > 0.001f)
    {
        FaceDirection(_path.CurrentDirection, delta);
    }
}
    
    private void OnTargetDetected(Node3D target)
    {
        _path.MoveTo(target);
    }
    
    private void OnTargetReached(Node3D target)
    {
        _attack.TryAttack(target);
    }
    
    public void setMainTarget(Node3D target)
    {
        goal = target;
        _path.SetMainTarget(goal);
    }
    
    public void onSetTargetDeleted()
    {
        _path.SetMainTarget(goal);
    }
}