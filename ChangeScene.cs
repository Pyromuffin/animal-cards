using Godot;
using System;

public partial class ChangeScene : Node
{
	public void ChangeToTutorial()
	{
		GetTree().ChangeSceneToFile("res://TutorialScene.tscn");
	}

	public void ChangeToGame()
	{
		GetTree().ChangeSceneToFile("res://node_2d.tscn");
	}
}
