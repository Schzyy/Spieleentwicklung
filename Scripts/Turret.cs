using Godot;
using System;
using System.Collections.Generic;

public partial class Turret : Node3D
{
    [Export] public float range = 10f;
    [Export] public float rotationSpeed = 3f;

    private bool _active = true;
    private MeshInstance3D _cannonHead;
    private Node3D _bulletHole;
    private MeshInstance3D _debugLineInstance;
    private ImmediateMesh _debugLineMesh;

    private AttackComponent _attackComponent;

    public override void _Ready()
    {
        _cannonHead = GetNode<MeshInstance3D>("CannonShape");
        _bulletHole = _cannonHead.GetNode<Node3D>("BulletPoint");
        _attackComponent = GetNode<AttackComponent>("AttackComponent");
        _debugLineMesh = new ImmediateMesh();
        _debugLineInstance = new MeshInstance3D
        {
            Mesh = _debugLineMesh
        };
        AddChild(_debugLineInstance);
    }

    public override void _Process(double delta)
    {
        if (!_active)
            return;
        Scout(delta);
    }

    private void Scout(double delta)
    {
        var spaceState = GetWorld3D().DirectSpaceState;
        var sphereShape = new SphereShape3D { Radius = range };
        var query = new PhysicsShapeQueryParameters3D
        {
            Shape = sphereShape,
            Transform = new Transform3D(Basis.Identity, GlobalPosition),
            CollideWithBodies = true,
            CollideWithAreas = false,
        };

        var results = spaceState.IntersectShape(query, maxResults: 32);

        Node3D closestEnemy = null;
        float closestDist = float.MaxValue;

        foreach (var result in results)
        {
            if (result.TryGetValue("collider", out var colliderObj))
            {
                var collider = colliderObj.AsGodotObject();
                if (collider is CollisionObject3D collisionObject)
                {
                    Node checkNode = collisionObject;
                    if (!checkNode.IsInGroup("Enemy") && checkNode.GetParent() != null)
                        checkNode = checkNode.GetParent();

                    if (checkNode.IsInGroup("Enemy") && checkNode is Node3D enemyNode)
                    {
                        float dist = GlobalPosition.DistanceTo(enemyNode.GlobalPosition);
                        if (dist < closestDist)
                        {
                            closestDist = dist;
                            closestEnemy = enemyNode;
                        }
                    }
                }
            }
        }

        DrawDebugLine(closestEnemy);

        if (closestEnemy != null)
            AlignCannon(closestEnemy, delta);
    }

    private void AlignCannon(Node3D enemy, double delta)
    {
        Vector3 toTarget = enemy.GlobalPosition - GlobalPosition;
        toTarget.Y = 0;
        if (toTarget.LengthSquared() < 0.0001f)
            return;

        toTarget = toTarget.Normalized();
        Basis targetBasis = Basis.LookingAt(toTarget, Vector3.Up).Orthonormalized();
        Basis newBasis = GlobalTransform.Basis.Orthonormalized().Slerp(targetBasis, (float)(rotationSpeed * delta));
        GlobalTransform = new Transform3D(newBasis, GlobalTransform.Origin);

        _attackComponent.TryAttack(enemy);
    }

    public void SetActive() => _active = true;

    public void SetInactive() => _active = false;

    private void DrawDebugLine(Node3D enemy)
    {
        _debugLineMesh.ClearSurfaces();
        if (enemy == null)
            return;

        _debugLineInstance.MaterialOverride = new StandardMaterial3D
        {
            AlbedoColor = Colors.Red,
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
            VertexColorUseAsAlbedo = true
        };

        _debugLineMesh.SurfaceBegin(Mesh.PrimitiveType.Lines);
        _debugLineMesh.SurfaceAddVertex(_debugLineInstance.ToLocal(_bulletHole.GlobalPosition));
        _debugLineMesh.SurfaceAddVertex(_debugLineInstance.ToLocal(enemy.GlobalPosition));
        _debugLineMesh.SurfaceEnd();
    }
}
