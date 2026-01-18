using Godot;
using System;

public partial class MeleeAttackComponent : Area3D
{
    [Export] private float attackRange = 4f;
    [Export] private float attackCooldown = 2f;
    private bool _active;
    private bool _inRange;
}
