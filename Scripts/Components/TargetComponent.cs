using Godot;
using System;
public partial class TargetComponent : Node3D
{
    private Node3D _currentTarget;
    public event Action<Node3D> targetChanged;
    public void onTargetEvaluated(Node3D target)
    {

        GD.Print(target + " inside targetComp");
        if(_currentTarget == target)
        {
            return;
        }
        GD.Print(_currentTarget + " " + target);
        _currentTarget = target;
        targetChanged?.Invoke(target);
    }
    public void mainPOI(Node3D goal)
    {
        _currentTarget = goal;
    }

}
