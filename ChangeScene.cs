using Godot;
using System;

public partial class ChangeScene : Node
{
	[Export] RichTextLabel label;
	
	[Export] AudioStreamPlayer audioStreamPlayer;

    public override void _Ready()
    {
		if(label == null)
			return;

        var bird = "res://art/ui/icons/icon_bird.png";
		var carnivore = "res://art/ui/icons/icon_carnivore.png";
		var coldBlooded = "res://art/ui/icons/icon_coldblooded.png";
		var flying = "res://art/ui/icons/icon_flying.png";
		var ground = "res://art/ui/icons/icon_ground.png";
		var herbivore = "res://art/ui/icons/icon_herbivore.png";
		var mammal = "res://art/ui/icons/icon_mammal.png";
		var water = "res://art/ui/icons/icon_water.png";
		var dinosaur = "res://art/ui/icons/icon_dinosaur.png";

		var replaced = label.Text.Replace("[B]", "[img width=24 height =24]" + bird + "[/img]");
		replaced = replaced.Replace("[C]", "[img width=24 height =24]" + carnivore + "[/img]");
		replaced = replaced.Replace("[CB]", "[img width=24 height =24]" + coldBlooded + "[/img]");
		replaced = replaced.Replace("[F]", "[img width=24 height =24]" + flying + "[/img]");
		replaced = replaced.Replace("[G]", "[img width=24 height =24]" + ground + "[/img]");
		replaced = replaced.Replace("[H]", "[img width=24 height =24]" + herbivore + "[/img]");
		replaced = replaced.Replace("[M]", "[img width=24 height =24]" + mammal + "[/img]");
		replaced = replaced.Replace("[W]", "[img width=24 height =24]" + water + "[/img]");
		replaced = replaced.Replace("[D]", "[img width=24 height =24]" + dinosaur + "[/img]");

		label.Text = replaced;
    }

	public void ChangeToTitle()
	{
		GetTree().ChangeSceneToFile("res://TitleScene.tscn");
		MusicTracker.MusicProgress = audioStreamPlayer.GetPlaybackPosition();
	}

	public void ChangeToTutorial()
	{
		GetTree().ChangeSceneToFile("res://TutorialScene.tscn");
		MusicTracker.MusicProgress = audioStreamPlayer.GetPlaybackPosition();
	}

	public void ChangeToGame()
	{
		GetTree().ChangeSceneToFile("res://OnePatronScene.tscn");
		MusicTracker.MusicProgress = audioStreamPlayer.GetPlaybackPosition();
	}
}
