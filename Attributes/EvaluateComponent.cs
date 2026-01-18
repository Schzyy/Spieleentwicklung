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
    GD.Print("entered eval");
    if(_selfValue == null)
    {
        return;
    }
    var root = newEntry.GetParent<Node3D>();
    GD.Print("Evaluating: ", root.GetPath());
    foreach (Node child in root.GetChildren())
    {
        if (child is ValueComponent valueComponent &&
            _selfValue.score < valueComponent.score)
        {
            TargetEvaluated?.Invoke(root);
            }
        }
    }
}
