using Godot;
using System;

public interface IHitSource
{
    int Damage { get; }
    Node3D Owner { get; }
};
