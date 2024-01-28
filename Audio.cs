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
    [Export] public float fadeInTime, fadeOutTime;
    [Export] public float startFade, endFade;
    

    [Export] AudioStream percussion, bass, piano, rhythm, cardFlyoutSfx, cardSfx, shuffleSfx, dealSfx, discardSfx;
    [Export] AudioStreamWav drumRoll;

    [Export] AudioStreamPlayer percussionPlayer, bassPlayer, pianoPlayer, rhythmPlayer, sfxPlayer, cardPlayer;

    public static Audio audio;


    public void PlayCardSfx() {
        PlaySfx(cardSfx);
    }


    public void PlaySfx(AudioStream stream){
        var player = new AudioStreamPlayer();
        AddChild(player);
        player.Stream = stream;
        player.Play();
        player.Finished += () => player.QueueFree();
    }

    public void PlayCardFlyoutSfx() {
        PlaySfx(cardFlyoutSfx);
    }


    AudioStreamPlayer shuffle;

    public void PlayCardShuffleSfx() {
        shuffle = new AudioStreamPlayer();
        AddChild(shuffle);
        shuffle.Stream = shuffleSfx;
        shuffle.Play();
        
        shuffle.Finished += () => {
             shuffle.Play();
        };
    }

    public void StopCardShuffleSfx() {
        shuffle.Stop();
        shuffle.QueueFree();
    }


    public void PlayCardDealSfx() {
        PlaySfx(dealSfx);
    }

    public void PlayDiscardSfx() {
        PlaySfx(discardSfx);
    }

    void SetRhythmVolume(float volume){
        AudioServer.SetBusVolumeDb(2, volume);
        AudioServer.SetBusVolumeDb(3, volume);
    }

    public void QueueRhythm() {
        AudioServer.SetBusMute(2, false);
        AudioServer.SetBusMute(3, false);
        var tween = CreateTween();
        tween.TweenMethod(Callable.From<float>(SetRhythmVolume), startFade, endFade, fadeInTime);

    }


    public void MuteRhythm() {
        AudioServer.SetBusMute(2, true);
        AudioServer.SetBusMute(3, true);
    }


    public void PlayPunchline(){

        sfxPlayer.Stream = drumRoll;
        sfxPlayer.Play();
        sfxPlayer.Seek(3.5f);

        if(!AudioServer.IsBusMute(2)){
            var tween = CreateTween();
            tween.TweenMethod(Callable.From<float>(SetRhythmVolume), endFade, startFade, fadeOutTime);
            tween.TweenCallback(Callable.From(MuteRhythm));
        }

        
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
