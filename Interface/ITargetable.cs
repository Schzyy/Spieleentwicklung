using Godot;
using System;

public interface ITargetable
{
    event Action<Node3D> TargetDestroyed;
    Node3D AsNode();
}