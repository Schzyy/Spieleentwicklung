using Godot;

public partial class Spawner : Node3D
{
    [Export] public PackedScene EnemyScene;
    [Export] public Timer timer;
    [Export] public Node3D castle;
    [Export] public Vector3 boxSize = new Vector3(5,5,5);
    private BoxShape3D _boxShape;
    private MeshInstance3D _mesh;

    public override void _Ready()
    {
        _mesh = new MeshInstance3D();
        _mesh.Mesh = new BoxMesh
        {
          Size = boxSize  
        };
        _mesh.MaterialOverride = new StandardMaterial3D
        {
            AlbedoColor = new Color(0,1,0, 0.3f),
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha
        };
        AddChild(_mesh);

        _boxShape = new BoxShape3D();
        _boxShape.Size = boxSize;
        var timer = GetNode<Timer>("Timer");
        timer.Timeout += SpawnEnemy;
        timer.WaitTime = 4.0f;
        timer.Start();
    }

    private void SpawnEnemy()
    {
        var spawnpoint = getRandomPointInBox();
        Enemy ene = (Enemy)EnemyScene.Instantiate();
        ene._Ready();
        AddChild(ene);
        ene.setMainTarget(castle);
        ene.GlobalTransform = new Transform3D(ene.GlobalTransform.Basis, spawnpoint);
    }
    private Vector3 getRandomPointInBox()
    {
        if(_boxShape == null)
        {
            return Vector3.Zero;
        }
        Vector3 extents = _boxShape.Size/2;
        float x = (float)GD.RandRange(-extents.X, extents.X);
        float y = 0.5f;
        float z = (float)GD.RandRange(-extents.Z, extents.Z);
        return GlobalTransform * new Vector3(x, y, z);
    }
 }
