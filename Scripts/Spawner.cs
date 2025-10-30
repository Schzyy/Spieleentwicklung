using Godot;
using System;

public partial class Spawner : Node3D
{
    [Export] public PackedScene EnemyScene;
    [Export] public Node3D spawnpoint;
    public override void _Ready()
    {
        SpawnEnemy();
    }
    private void SpawnEnemy()
    {
        var ene = (Enemy)EnemyScene.Instantiate();
        AddChild(ene);
        ene.setTarget(GetNode<Node3D>("Target"));
        ene.GlobalTransform = new Transform3D(ene.GlobalTransform.Basis, spawnpoint.GlobalTransform.Origin);
    }
}
