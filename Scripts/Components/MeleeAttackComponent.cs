using Godot;
using System;
using System.Threading.Tasks;

public partial class MeleeAttackComponent : Area3D, IHitSource
{
    [Export] private NodePath _hitboxPath;
    [Export] public int damage = 20;
    public int Damage => damage;
    public Node3D Owner => GetParent<Node3D>();
    private CollisionShape3D _hitbox;
    private CollisionShape3D _rangeCollision;
    private Area3D _rangeArea;
    private CylinderShape3D _cylinderShape;
    [Export] private float attackRange = 4f;
    [Export] private float attackCooldown = 2f;
    private bool _canAttack = true;
    private bool _inRange;
    private float _attackDuration = 0.3f;

    // RangeLogic if in range attack if possible -> cooldown not already attacking
    // 
    public override void _Ready()
    {
        init();
        AreaEntered += OnAreaEntered;
    } 
    private void init()
    {
        _hitbox = GetNode<CollisionShape3D>(_hitboxPath);
        _hitbox.Disabled = true;

        _rangeArea = new Area3D();
        _rangeCollision = new CollisionShape3D();
        _cylinderShape = new CylinderShape3D();
        _cylinderShape.Radius = attackRange;
        _cylinderShape.Height = 0.2f;
        _rangeCollision.Shape = _cylinderShape;
        _rangeCollision.DebugColor = new Color(1,0,0,0.4f);
        _rangeCollision.Translate(new Vector3(0,0.5f,0));
        _rangeArea.AddChild(_rangeCollision);
        AddChild(_rangeArea);

        _rangeArea.BodyEntered += inRange;
        _rangeArea.BodyExited += outOfRange;
    }
    //what happens if something enters it range? -> it says that we can attack
    //set hitbox as acitve for some time and set it to cooldown
    private void inRange(Node3D target)
    {
        _inRange = true;
    }
    private void outOfRange(Node3D target)
    {
        _inRange = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(_inRange == true && _canAttack == true)
        {
            attack();
        }

    }
    private async void attack()
    {
        _canAttack = false;
        _hitbox.Disabled = false;

        Timer hit = new Timer();
        hit.WaitTime = _attackDuration;
        hit.OneShot = true;
        AddChild(hit);
        hit.Start();

        await ToSignal(hit, "timeout");
        _hitbox.Disabled = true;
        hit.QueueFree();

        Timer cd = new Timer();
        cd.WaitTime = attackCooldown;
        cd.OneShot = true;
        AddChild(cd);
        cd.Start();
        await ToSignal(cd, "timeout");
        cd.QueueFree();

        _canAttack = true;
    }
    private void OnAreaEntered(Area3D area)
    {
        if(area.HasMethod("OnHit"))
        {
            area.CallDeferred("OnHit", this);
        }
    }
}
