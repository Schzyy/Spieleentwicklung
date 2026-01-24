using Godot;
using System;
public partial class TargetComponent : Node3D
{
    private Node3D _currentTarget;
    private ITargetable _targetable;
    public event Action<Node3D> targetChanged;
    public event Action targetDied;
    public void onTargetEvaluated(Node3D target)
    {
        if(_currentTarget == target)
        {
            return;
        }   
        _currentTarget = target;
        foreach (Node child in target.GetChildren())
        {
            if (child is TargetableComponent targetable)
            {
                _targetable = targetable;
                _targetable.TargetDestroyed += onTargetDestroyed;
            }
        }
        targetChanged?.Invoke(target);
    }
    public void mainPOI(Node3D goal)
    {
        _currentTarget = goal;
    }
    public void onTargetDestroyed(Node3D deadTarget)
    {
        if(_currentTarget == deadTarget)
        {
            ClearTarget();
        }
    }

    private void ClearTarget()
    {
        if(_targetable != null)
        {
            _targetable.TargetDestroyed -= onTargetDestroyed;
            _targetable = null;
        }
    }

}
