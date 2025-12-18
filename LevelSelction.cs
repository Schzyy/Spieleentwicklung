using Godot;
using System;

public partial class LevelSelction : VBoxContainer
{
    private bool _show = false;
    private Button _tutorial;
    private Button _levelOne;
    private Button _levelTwo;
    private Button _levelThree;

    public override void _Ready()
    {
        Visible = _show;

        _tutorial = GetNode<Button>("Tutorial");
        _levelOne = GetNode<Button>("LevelOne");
        _levelTwo = GetNode<Button>("LevelTwo");
        _levelThree = GetNode<Button>("LevelThree");
    
        _tutorial.Pressed += onTutorial;
        _levelOne.Pressed += onLevelOne;
        _levelTwo.Pressed += onLevelTwo;
        _levelThree.Pressed += onLevelThree;

        
    }

    public void toggleVisibility()
    {
        _show = !_show;
        Visible = _show;
    }

    private void onTutorial()
    {
    }
    private void onLevelOne()
    {
        GetTree().ChangeSceneToFile("res://Maps/level_one.tscn");
    }
    private void onLevelTwo()
    {   
    }
    private void onLevelThree()
    {   
    }
}
