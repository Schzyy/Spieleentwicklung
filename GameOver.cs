using Godot;
using System;

public partial class GameOver : Control
{
    private Button _homeMenu;
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;

        _homeMenu = GetNode<Button>("HomeButton");

        _homeMenu.Pressed += OnHomeButton;
    }

    private void OnHomeButton()
    {
        GetTree().ChangeSceneToFile("res://Home.tscn");
    }
}
