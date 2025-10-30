using Godot;
using System;
using System.Collections.Generic;

public partial class DetectionComponent : Node3D
{
    [Export] public float range = 10f;
    [Export] public Godot.Collections.Array<string> GroupMembers { get; set; } = new Godot.Collections.Array<string> { };
    [Export] public bool debug = true;
    private Vector3 position;
    private MeshInstance3D debugLineInstance;
    private ImmediateMesh debugLineMesh;

    public override void _Ready()
    {        
    }
    private void scout(double delta)
    {
        var spaceState = GetWorld3D().DirectSpaceState;
        var sphereShape = new SphereShape3D
        {
            Radius = range
        };

        var query = new PhysicsShapeQueryParameters3D
        {
            Shape = sphereShape,
            Transform = new Transform3D(Basis.Identity, position),
            CollideWithBodies = true,
            CollideWithAreas = true
        };

        var results = spaceState.IntersectShape(query, maxResults: 32);

        foreach (var result in results)
        {
            if (result.TryGetValue("collider", out var colliderObj))
            {
                var collider = colliderObj.AsGodotObject() as Node3D;

                // if (collider != null && collider.IsInGroup(GroupMembers.Contains))
                {
                    float dist = GlobalPosition.DistanceTo(collider.GlobalPosition);
                }
            }
        }
    }

    private void DrawDebugLine(Node3D target)
    {
        debugLineMesh.ClearSurfaces();
        StandardMaterial3D lineMat = new StandardMaterial3D
        {
            AlbedoColor = Colors.Red,
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
            VertexColorUseAsAlbedo = true
        };
        debugLineInstance.MaterialOverride = lineMat;
        debugLineMesh.SurfaceBegin(Mesh.PrimitiveType.Lines);
        Vector3 start = debugLineInstance.ToLocal(GlobalPosition);
        Vector3 end = debugLineInstance.ToLocal(target.GlobalPosition);
        debugLineMesh.SurfaceAddVertex(start);
        debugLineMesh.SurfaceAddVertex(end);
        debugLineMesh.SurfaceEnd();
    }
}
