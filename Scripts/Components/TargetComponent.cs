using Godot;
using System;

public partial class TargetComponent : Node3D
{
    private Node3D _currentTarget;
    private ITargetable _targetable;

    public event Action<Node3D> TargetChanged;
    public event Action TargetLost;

    public Node3D CurrentTarget => _currentTarget;

    /// <summary>
    /// Evaluates a detection candidate and sets it as the current target if it is targetable.
    /// The candidate's parent is checked for an ITargetable child (e.g. HealthComponent).
    /// </summary>
    public void SetTarget(Node3D candidate)
    {
        var root = candidate.GetParent() as Node3D;
        if (root == null)
            return;

        if (_currentTarget == root)
            return;

        ITargetable targetable = null;
        foreach (Node child in root.GetChildren())
        {
            if (child is ITargetable t)
            {
                targetable = t;
                break;
            }
        }

        if (targetable == null)
            return;

        ClearTarget();

        _currentTarget = root;
        _targetable = targetable;
        _targetable.TargetDestroyed += OnTargetDestroyed;

        TargetChanged?.Invoke(root);
    }

    public void SetMainTarget(Node3D goal)
    {
        _currentTarget = goal;
    }

    public void OnTargetDestroyed(Node3D deadTarget)
    {
        GD.Print("target died");

        if (_currentTarget == deadTarget)
        {
            ClearTarget();
            TargetLost?.Invoke();
        }
    }

    private void ClearTarget()
    {
        if (_targetable != null)
        {
            _targetable.TargetDestroyed -= OnTargetDestroyed;
            _targetable = null;
        }
        _currentTarget = null;
    }
}
