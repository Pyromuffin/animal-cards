/*
    drums on the tile
    bass in the game
    play a setup card add rhythm and piano whilst setting up



*/

using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Audio : Node {

    [Export] public int bpm;
    [Export] public float fadeTime;
    [Export] public float startFade, endFade;
    

    [Export] AudioStreamMP3 percussion, bass, piano, rhythm;

    [Export] AudioStreamPlayer percussionPlayer, bassPlayer, pianoPlayer, rhythmPlayer;

    public static Audio audio;


    void SetRhythmVolume(float volume){
        AudioServer.SetBusVolumeDb(2, volume);
        AudioServer.SetBusVolumeDb(3, volume);
    }

    public void QueueRhythm() {
        AudioServer.SetBusMute(2, false);
        AudioServer.SetBusMute(3, false);
        var tween = CreateTween();
        tween.TweenMethod(Callable.From<float>(SetRhythmVolume), startFade, endFade, fadeTime);

    }


    public void MuteRhythm() {
        AudioServer.SetBusMute(2, true);
        AudioServer.SetBusMute(3, true);
    }

    public override void _Ready()
    {
        audio = this;

        percussionPlayer.Stream = percussion;
        percussionPlayer.Play();

        bassPlayer.Stream = bass;
        bassPlayer.Play();

        pianoPlayer.Stream = piano;
        pianoPlayer.Play();

        rhythmPlayer.Stream = rhythm;
        rhythmPlayer.Play();

       MuteRhythm();
    }




}
