using Godot;
using System;

public partial class HomeScreen : Control
{
    [Export] public LevelSelction levelSelector;
    private Button _startButton;
    private Button _quitButton;

    public override void _Ready()
    {
        _startButton = GetNode<Button>("MainSelection/StartButton");
        _quitButton = GetNode<Button>("MainSelection/QuitButton");

        _startButton.Pressed += onStart;
        _quitButton.Pressed += onQuit;
    }
    private void onStart()
    {
        levelSelector.toggleVisibility();
    }
    private void onQuit()
    {
        GetTree().Quit();
    }
}

//ToDo funktionalität zu den Szenen erweitern, Design