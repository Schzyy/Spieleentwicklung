using Godot;
using System;

public partial class ValueComponent : Node3D
{
    [Export] public int targetValue;
    public int score => targetValue;
}
