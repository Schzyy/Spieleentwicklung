using Godot;
using System;

public partial class DetectionComponent : Node3D
{
    [Export] public float range = 10f;
    [Export] public bool detectEnemy = false;
    [Export] public bool detectBreakable = false;
    [Export] public bool detectPlayer = false;
    [Export] public bool detectTurret = false;
    [Export] public bool debug = true;

    public event Action<Node3D> targetDetected;

    private MeshInstance3D _debugSphere;

    public override void _Ready()
    {
        if (debug)
            CreateDebugSphere();

        GD.Print($"Detection ready. Range={range}, Debug={debug}");
    }

    public override void _PhysicsProcess(double delta)
    {
        Scout();
        if (_debugSphere != null)
            _debugSphere.GlobalPosition = GlobalPosition;
    }

    private void Scout()
    {
        var sphere = new SphereShape3D { Radius = range };

        var query = new PhysicsShapeQueryParameters3D
        {
            Shape = sphere,
            Transform = GlobalTransform,
            CollideWithBodies = true,
            CollideWithAreas = false,
        };

        var spaceState = GetWorld3D().DirectSpaceState;

        // Perform query
        var results = spaceState.IntersectShape(query, maxResults: 64);

        if (results == null || results.Count == 0)
        {
            GD.Print("Detection: no results from IntersectShape.");
            return;
        }

        GD.Print($"Detection: found {results.Count} results.");

        foreach (var result in results)
        {
            if (!result.TryGetValue("collider", out var colliderObj))
                continue;

            // In Godot 4 die Rückgabe ist ein Object (CollisionObject3D, Area3D, etc.)
            var collObj = colliderObj.AsGodotObject() as CollisionObject3D;
            if (collObj == null)
            {
                GD.Print("Detection: collider is not a CollisionObject3D");
                continue;
            }

            // Log hilfreiche Infos
            uint objLayer = collObj.CollisionLayer;
            uint objMask = collObj.CollisionMask;
            GD.Print($" -> Detected: {collObj.Name} Type={collObj.GetType().Name} Layer={objLayer} Mask={objMask} Pos={collObj.GlobalPosition} Groups={collObj.GetGroups()}");

            // Versuche Node3D cast (dein Event erwartet Node3D)
            var asNode3D = collObj as Node3D;
            if (asNode3D == null)
            {
                // Falls nicht Node3D (sollte selten sein), skip
                GD.Print("   (collider is not Node3D, skipping event)");
                continue;
            }

            // Gruppen-Checks wie vorher
            if (asNode3D.IsInGroup("Enemy") && detectEnemy)
                targetDetected?.Invoke(asNode3D);

            if (asNode3D.IsInGroup("Breakable") && detectBreakable)
            {
                GD.Print("   -> breakable found");
                targetDetected?.Invoke(asNode3D);
            }

            if (asNode3D.IsInGroup("Turret") && detectTurret)
                targetDetected?.Invoke(asNode3D);

            if (asNode3D.IsInGroup("Player") && detectPlayer)
                targetDetected?.Invoke(asNode3D);
        }
    }

    private void CreateDebugSphere()
    {
        // Entferne alte falls vorhanden
        if (_debugSphere != null)
            _debugSphere.QueueFree();

        _debugSphere = new MeshInstance3D();
        var mesh = new SphereMesh
        {
            Radius = range,
            RadialSegments = 24,
            Rings = 16
        };
        _debugSphere.Mesh = mesh;

        var mat = new StandardMaterial3D
        {
            AlbedoColor = new Color(0f, 1f, 0f, 0.15f),
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
        };

        _debugSphere.MaterialOverride = mat;

        // Damit die Sphäre nicht kollidiert
        _debugSphere.SetPhysicsProcess(false);
        _debugSphere.SetProcess(false);

        AddChild(_debugSphere);
        _debugSphere.Position = Vector3.Zero; // child at origin -> matches GlobalPosition via parent transform
    }
}
