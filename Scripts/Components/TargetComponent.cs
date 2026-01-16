using Godot;
using System;
public partial class TargetComponent : Node3D
{
    private Node3D _currentTarget;
    public event Action<Node3D> targetChanged;
    public void onTargetEvaluated(Node3D target)
    {
        if(_currentTarget == target)
        {
            return;
        }
        _currentTarget = target;
        targetChanged?.Invoke(target);
    }

}
