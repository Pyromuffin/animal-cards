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

        Game.ReplaceText( label );
    }

	public void ChangeToTitle()
	{
		GetTree().ChangeSceneToFile("res://TitleScene.tscn");
		MusicTracker.MusicProgress = audioStreamPlayer.GetPlaybackPosition();
	}

	public void ChangeToTitleAndReset()
	{
		GetTree().ChangeSceneToFile("res://TitleScene.tscn");
		PlayerData.Reset();
		MusicTracker.MusicProgress = audioStreamPlayer.GetPlaybackPosition();
	}

	public void ChangeToTutorial()
	{
		GetTree().ChangeSceneToFile("res://TutorialScene.tscn");
		MusicTracker.MusicProgress = audioStreamPlayer.GetPlaybackPosition();
	}

	public void ChangeToStory()
	{
		GetTree().ChangeSceneToFile("res://StoryScene.tscn");
		MusicTracker.MusicProgress = audioStreamPlayer.GetPlaybackPosition();
	}

	public void ChangeToCredits()
	{
		GetTree().ChangeSceneToFile("res://CreditsScene.tscn");
		MusicTracker.MusicProgress = audioStreamPlayer.GetPlaybackPosition();
	}

	public void ChangeToGame()
	{
		GetTree().ChangeSceneToFile(PlayerData.GetNextPatronScene());
		MusicTracker.MusicProgress = audioStreamPlayer.GetPlaybackPosition();
	}

	public void ChangeToScene( string targetScene )
	{
		GetTree().ChangeSceneToFile(targetScene);
		MusicTracker.MusicProgress = audioStreamPlayer.GetPlaybackPosition();
	}
}
