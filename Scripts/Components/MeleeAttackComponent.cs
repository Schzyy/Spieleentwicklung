using Godot;
using System;
using System.Threading.Tasks;

public partial class MeleeAttackComponent : Area3D, IHitSource
{
    [Export] private NodePath _hitboxPath;
    [Export] public int damage = 20;
    public int Damage => damage;

    Node3D IHitSource.Owner => throw new NotImplementedException();


    private CollisionShape3D _hitbox;
    private CylinderShape3D _cylinderShape;
    [Export] private float attackRange = 4f;
    [Export] private float attackCooldown = 2f;
    private bool _canAttack = true;
    private bool _inRange;
    private float _attackDuration = 0.3f;
    private Enemy _owner;
    private RayCast3D raycast;

    // RangeLogic if in range attack if possible -> cooldown not already attacking
    // 
    public override void _Ready()
    {
        _hitbox = GetNode<CollisionShape3D>(_hitboxPath);
        AreaEntered += OnAreaEntered;
        _owner = GetParent<Enemy>();
    } 
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
        var space = GetWorld3D().DirectSpaceState;
        Vector3 from = GlobalPosition;
        Vector3 to = from + GlobalTransform.Basis.Z  * -1.5f;
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        query.CollideWithAreas = true;
        query.CollisionMask = 1 << 4;
        var result = space.IntersectRay(query);
        DebugDraw3D.DrawLine(from, to, Colors.AliceBlue);
        if(result.Count > 0)
        {
            _inRange = true;
        } else
        {
            _inRange = false;
        }
        if(_inRange == true && _canAttack == true)
        {
            attack();
        }
    }
    public void EnableHitbox()
    {
        _hitbox.Disabled = false;
    }
    public void DisableHitbox()
    {
        _hitbox.Disabled = true;
    }
    public void AttackFinished()
    {
        _canAttack = true;
    }
    private async void attack()
    {
        _canAttack = false;
        _owner.playAttack();
    }
    private void OnAreaEntered(Area3D area)
    {
        if(area.HasMethod("OnHit"))
        {
            area.CallDeferred("OnHit", this);
        }
    }
}
