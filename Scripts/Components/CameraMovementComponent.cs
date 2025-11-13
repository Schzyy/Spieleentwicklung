using Godot;
using System;

public partial class CameraMovementComponent : Node3D
{
    [Export] public float MoveSpeed = 5.0f;
    [Export] public float MouseSensitivity = 0.1f;
    [Export] public bool UsePlanarMovement = false; // true = ignore pitch (XZ only)
    [Export] public float BoostMultiplier = 3.0f;

    private float _pitch = 0.0f;
    private float _yaw = 0.0f;
    private Camera3D _camera;

    public override void _Ready()
    {
        _camera = GetParent() as Camera3D;
        if (_camera == null)
        {
            return;
        }
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            _yaw -= mouseMotion.Relative.X * MouseSensitivity;
            _pitch -= mouseMotion.Relative.Y * MouseSensitivity;

            _pitch = Mathf.Clamp(_pitch, -89f, 89f);

            if (_camera != null)
            {
                _camera.RotationDegrees = new Vector3(_pitch, _yaw, 0f);
            }
        }

        if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured ? Input.MouseModeEnum.Visible: Input.MouseModeEnum.Captured;
        }
    }

    public override void _Process(double delta)
    {
        if (_camera == null)
        {
            return;
        }
        Vector3 camRight = _camera.GlobalTransform.Basis.X;  
        Vector3 camUp = _camera.GlobalTransform.Basis.Y;
        Vector3 camForward = -_camera.GlobalTransform.Basis.Z;
        if (UsePlanarMovement)
        {
            camForward = new Vector3(camForward.X, 0f, camForward.Z).Normalized();
            camRight = new Vector3(camRight.X, 0f, camRight.Z).Normalized();
        }

        Vector3 moveDir = Vector3.Zero;

        if (Input.IsActionPressed("move_forward"))
        {
            moveDir += camForward;
        }
        if (Input.IsActionPressed("move_backward"))
        {
            moveDir -= camForward;
        }
        if (Input.IsActionPressed("move_right"))
        {
            moveDir += camRight;
        }
        if (Input.IsActionPressed("move_left"))
        {
            moveDir -= camRight;
        }
        if (Input.IsActionPressed("move_up"))
        {
            moveDir += camUp;
        }
        if (Input.IsActionPressed("move_down"))
        {
            moveDir -= camUp;
        }

        if (moveDir != Vector3.Zero)
        {
            moveDir = moveDir.Normalized();

            float speed = MoveSpeed;
            if (Input.IsActionPressed("ui_shift"))
            {
                speed *= BoostMultiplier;
            }

            _camera.GlobalTranslate(moveDir * speed * (float)delta);
        }
    }
}
