using Godot;
using System;

public partial class EvaluateComponent : Node3D
{
    [Export] private ValueComponent _selfValue;
    public event Action<Node3D> TargetEvaluated;
    public void eval(Node3D newEntry)
    {
        foreach(Node child in newEntry.GetChildren())
        {
            if(child is ValueComponent valueComponent && _selfValue.score < valueComponent.score)
            {
                TargetEvaluated?.Invoke(newEntry);
            }    
        }
    }
}
