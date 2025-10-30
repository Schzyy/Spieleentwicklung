using Godot;
using System;
using System.Collections.Generic;

public partial class Turret : Node3D
{
    [Export] public float range = 10f;
    [Export] public float rotationSpeed = 3f;

    private Vector3 position;
    private MeshInstance3D cannonHead;
    private Node3D bulletHole;
    private MeshInstance3D debugLineInstance;
    private ImmediateMesh debugLineMesh;

    public override void _Ready()
    {
        position = GlobalTransform.Origin;
        cannonHead = GetNode<MeshInstance3D>("CannonShape");
        bulletHole = cannonHead.GetNode<Node3D>("BulletPoint");
        debugLineMesh = new ImmediateMesh();
        debugLineInstance = new MeshInstance3D
        {
            Mesh = debugLineMesh
        };
        AddChild(debugLineInstance);
    }

    public override void _Process(double delta)
    {
        setScoutRange();
        scout(delta);
    }

    private void setScoutRange()
    {
        float area = Mathf.Pi * range * range;
    }

    private void scout(double delta)
    {
        var spaceState = GetWorld3D().DirectSpaceState;
        var sphereShape = new SphereShape3D { Radius = range };

        var query = new PhysicsShapeQueryParameters3D
        {
            Shape = sphereShape,
            Transform = new Transform3D(Basis.Identity, position),
            CollideWithBodies = true,
            CollideWithAreas = true
        };

        var results = spaceState.IntersectShape(query, maxResults: 32);

        Node3D closestEnemy = null;
        float closestDist = float.MaxValue;

        foreach (var result in results)
        {
            if (result.TryGetValue("collider", out var colliderObj))
            {
                var collider = colliderObj.AsGodotObject() as Node3D;

                if (collider != null && collider.IsInGroup("Enemy"))
                {
                    float dist = GlobalPosition.DistanceTo(collider.GlobalPosition);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestEnemy = collider;
                    }
                }
            }
        }

        // Draw debug line to closest enemy
        DrawDebugLine(closestEnemy);

        if (closestEnemy != null)
        {
            alignCannon(closestEnemy.GlobalPosition, delta);
        }
    }

    private void alignCannon(Vector3 enemyPos, double delta)
    {
        Vector3 toTarget = enemyPos - GlobalPosition;
        toTarget.Y = 0;
        if (toTarget.LengthSquared() < 0.0001f)
        {
            return;
        }    
        toTarget = toTarget.Normalized();
        Basis targetBasis = Basis.LookingAt(toTarget, Vector3.Up);
        Basis currentBasis = GlobalTransform.Basis.Orthonormalized();
        targetBasis = targetBasis.Orthonormalized();
        Basis newBasis = currentBasis.Slerp(targetBasis, (float)(rotationSpeed * delta));
        GlobalTransform = new Transform3D(newBasis, GlobalTransform.Origin);
    }

private void DrawDebugLine(Node3D enemy)
{
    debugLineMesh.ClearSurfaces();

    if (enemy == null)
        return;

    StandardMaterial3D lineMat = new StandardMaterial3D
    {
        AlbedoColor = Colors.Red,
        ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
        VertexColorUseAsAlbedo = true
    };
    debugLineInstance.MaterialOverride = lineMat;

    debugLineMesh.SurfaceBegin(Mesh.PrimitiveType.Lines);

    Vector3 start = debugLineInstance.ToLocal(bulletHole.GlobalPosition);
    Vector3 end = debugLineInstance.ToLocal(enemy.GlobalPosition);

    debugLineMesh.SurfaceAddVertex(start);
    debugLineMesh.SurfaceAddVertex(end);

    debugLineMesh.SurfaceEnd();
}

}
