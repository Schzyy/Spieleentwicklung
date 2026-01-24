using Godot;
using System;

public partial class EvaluateComponent : Node3D
{
    private ValueComponent _selfValue; 
    public event Action<Node3D> TargetEvaluated;
    public void attachValue(ValueComponent ownValue)
    {
        _selfValue = ownValue;
    }
    public void eval(Node3D newEntry)
    {
    if(_selfValue == null)
    {
        return;
    }
    var root = newEntry.GetParent<Node3D>();
    foreach (Node child in root.GetChildren())
    {
        GD.Print(newEntry.Name);
        if (child is ValueComponent valueComponent)
        {
            GD.Print("evaluated this fella " + root.Name);
            TargetEvaluated?.Invoke(root);
        }
    }
    }
}
