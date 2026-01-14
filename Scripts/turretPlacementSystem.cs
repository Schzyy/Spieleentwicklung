using Godot;
using System;

public partial class turretPlacementSystem : Node3D
{
    [Export] public PackedScene TurretScene;
    [Export] public float MaxPlacementDistance = 50f;
    [Export] public Color ValidPlacementColor = new Color(0, 1, 0, 0.5f); // Green transparent
    [Export] public Color InvalidPlacementColor = new Color(1, 0, 0, 0.5f); // Red transparent
    [Export] public uint PlacementLayerMask = 1; // Layer 1 for ground
    
    private Camera3D _camera;
    private Node3D _previewTurret;
    private MeshInstance3D _previewMesh;
    private bool _canPlace = false;
    private Vector3 _placementPosition;
    
    public override void _Ready()
    {        
        _camera = GetViewport().GetCamera3D();
        
        if (TurretScene != null)
        {
            CreatePreviewTurret();
        }
    }
    
    private void CreatePreviewTurret()
    {
        var turretInstance = TurretScene.Instantiate<Turret>();
        turretInstance.setInactive();
        _previewTurret = new Node3D();
        AddChild(_previewTurret);
        _previewTurret.AddChild(turretInstance);
        MakeTransparent(_previewTurret, ValidPlacementColor);
        _previewTurret.Visible = false;
    }
    
    private void MakeTransparent(Node node, Color color)
    {
        if (node is MeshInstance3D meshInstance)
        {
            var material = new StandardMaterial3D
            {
                Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
                AlbedoColor = color,
                ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded
            };
            meshInstance.MaterialOverride = material;
        }
        
        foreach (Node child in node.GetChildren())
        {
            MakeTransparent(child, color);
        }
    }
    
    public override void _Process(double delta)
    {
        UpdatePlacementPreview();
        
        if (Input.IsActionJustPressed("place_turret"))
        {
            TryPlaceTurret();
        }
        
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            _previewTurret.Visible = false;
        }
    }
    
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("toggle_placement"))
        {
            _previewTurret.Visible = !_previewTurret.Visible;
        }
    }
    
    private void UpdatePlacementPreview()
    {
        if (!_previewTurret.Visible) return;
        
        var spaceState = GetWorld3D().DirectSpaceState;
        var viewport = GetViewport();
        var screenCenter = viewport.GetVisibleRect().Size / 2;
        
        var from = _camera.ProjectRayOrigin(screenCenter);
        var to = from + _camera.ProjectRayNormal(screenCenter) * MaxPlacementDistance;
        
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        query.CollisionMask = PlacementLayerMask;
        
        var result = spaceState.IntersectRay(query);
        
        if (result.Count > 0)
        {
            Vector3 hitPosition = (Vector3)result["position"];
            Vector3 hitNormal = (Vector3)result["normal"];
            
            // Check if the surface is flat enough (ground-like)
            float normalDotUp = hitNormal.Dot(Vector3.Up);
            bool isFlatSurface = normalDotUp > 0.8f; 
            
            if (isFlatSurface)
            {
                // Snap to ground level - do an additional downward raycast to be sure
                Vector3 snapPosition = SnapToGround(hitPosition);
                snapPosition = SnapToGrid(snapPosition);
                GD.Print(snapPosition);

                _placementPosition = snapPosition;
                _previewTurret.GlobalPosition = _placementPosition;
                
                // Check if placement is valid (not overlapping with other turrets, etc.)
                _canPlace = IsValidPlacement(_placementPosition);
            }
            else
            {
                // Not a valid ground surface (wall, ceiling, steep slope)
                _canPlace = false;
                _previewTurret.GlobalPosition = hitPosition; // Show where you're aiming but mark as invalid
            }
            
            // Update color based on validity
            Color targetColor = _canPlace ? ValidPlacementColor : InvalidPlacementColor;
            MakeTransparent(_previewTurret, targetColor);
        }
        else
        {
            _canPlace = false;
            MakeTransparent(_previewTurret, InvalidPlacementColor);
        }
    }
    
    private Vector3 SnapToGround(Vector3 position)
    {
        var spaceState = GetWorld3D().DirectSpaceState;
        var from = position + Vector3.Up * 2f; 
        var to = position + Vector3.Down * 10f; 
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        query.CollisionMask = PlacementLayerMask;
        var result = spaceState.IntersectRay(query);
        if (result.Count > 0)
        {

            return (Vector3)result["position"];
        }
        return position;
    }
    private Vector3 SnapToGrid(Vector3 position)
{
        return new Vector3(
            Mathf.Round(position.X)+0.5f,
            Mathf.Round(position.Y),
            Mathf.Round(position.Z)
    );
}
    
    private bool IsValidPlacement(Vector3 position)
    {
        var spaceState = GetWorld3D().DirectSpaceState;
        var sphereShape = new SphereShape3D { Radius = 2f }; // Minimum distance between turrets
        var query = new PhysicsShapeQueryParameters3D
        {
            Shape = sphereShape,
            Transform = new Transform3D(Basis.Identity, position),
            CollideWithBodies = true,
            CollideWithAreas = true
        };
        var results = spaceState.IntersectShape(query, maxResults: 10);
        foreach (var result in results)
        {
            if (result.TryGetValue("collider", out var colliderObj))
            {
                var collider = colliderObj.AsGodotObject();
                if (collider is Node node && node.IsInGroup("Neutral"))
                {
                    return false;
                }
            }
        }
        return true;
    }
    
    private void TryPlaceTurret()
    {
        if (!_previewTurret.Visible || !_canPlace) return;
        var turret = TurretScene.Instantiate<Node3D>();
        GetTree().Root.AddChild(turret);
        turret.GlobalPosition = _placementPosition;
        turret.AddToGroup("Turret");
        GD.Print($"Turret placed at {_placementPosition}");
    }
}