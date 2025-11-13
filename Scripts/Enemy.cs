using Godot;

public partial class Enemy : CharacterBody3D
{
    private DetectionComponent _detection;
    private PathComponent _path;
    private AttackComponent _attack;

    private Node3D goal;
    public override void _Ready()
    {
        _detection = GetNode<DetectionComponent>("DetectionComponent");
        _path = GetNode<PathComponent>("PathComponent");
        _attack = GetNode<AttackComponent>("AttackComponent");

        _detection.targetDetected += OnTargetDetected;
        _path.TargetReached += OnTargetReached;
    }

    private void OnTargetDetected(Node3D target)
    {
        GD.Print("Happens");
        _path.MoveTo(target);
    }

    private void OnTargetReached(Node3D target)
    {
        _attack.TryAttack(target);
    }

    public void setMainTarget(Node3D target)
    {
        _path.setMainTarget(target);
    }
}
