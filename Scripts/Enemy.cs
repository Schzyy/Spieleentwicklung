using Godot;

public partial class Enemy : CharacterBody3D
{
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

    public override void _Process(double delta)
    {
        FaceTarget(goal.GlobalPosition);
    }


    public void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - GlobalPosition;
        direction.Y = 0; // Prevent tilting up/down

        if (direction.LengthSquared() < 0.001f)
            return;

        LookAt(GlobalPosition + direction, Vector3.Up);
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
