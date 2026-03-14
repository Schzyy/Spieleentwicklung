using Godot;

public partial class ValueComponent : Node3D
{
    [Export] public int TargetValue;
    public int Score => TargetValue;
}
