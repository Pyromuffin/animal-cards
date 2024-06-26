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
    

    [Export] AudioStream percussion, bass, piano, rhythm, cardFlyoutSfx, cardSfx, shuffleSfx, dealSfx, discardSfx, drumRoll, fail;

    [Export] AudioStreamPlayer percussionPlayer, bassPlayer, pianoPlayer, rhythmPlayer, sfxPlayer, cardPlayer, voicePlayer;

    [Export] public AudioStream[] punchlines;
    [Export] public AudioStream[] setups;

    public static Audio audio;

    [Export] public bool BassIsVolume;

    public void PlaySetupSfx(){
        var randomSetup = Random.Shared.Next() % setups.Length;
        voicePlayer.Stream = setups[randomSetup];
        voicePlayer.Play();
    }

    public void PlayPunchlineSfx() {
        var randomPunchline = Random.Shared.Next() % punchlines.Length;
        voicePlayer.Stream = punchlines[randomPunchline];
        voicePlayer.Play();
    }

    public void PlayCardSfx() {
        PlaySfx(cardSfx);
    }

    public void PlayDamageSfx(){
        PlaySfx(fail, 1, -10);
    }


    public void PlaySfx(AudioStream stream, float offset = 0, float volume = 0){
        var player = new AudioStreamPlayer();
        AddChild(player);
        player.Stream = stream;
        player.Play();
        player.Seek(offset);
        player.VolumeDb = volume;
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
        if( !BassIsVolume )
        {
            AudioServer.SetBusVolumeDb(2, volume);
        }
        
        AudioServer.SetBusVolumeDb(3, volume);
    }

    public void QueueRhythm() {
        if( !BassIsVolume )
        {
            AudioServer.SetBusMute(2, false);
        }
        
        AudioServer.SetBusMute(3, false);
        var tween = CreateTween();
        tween.TweenMethod(Callable.From<float>(SetRhythmVolume), startFade, endFade, fadeInTime);

    }


    public void MuteRhythm() {
        AudioServer.SetBusMute(2, !BassIsVolume);
        AudioServer.SetBusMute(3, true);
    }


    public void PlayPunchline(){
        sfxPlayer.Stream = drumRoll;
        sfxPlayer.Play();

        if(!AudioServer.IsBusMute(3)){
            var tween = CreateTween();
            tween.TweenMethod(Callable.From<float>(SetRhythmVolume), endFade, startFade, fadeOutTime);
            tween.TweenCallback(Callable.From(MuteRhythm));
        }

        
    }

    // Bass was getting out of sync so here's a big hack.
    public void ResetStream( AudioStreamPlayer audioStreamPlayer )
    {
        audioStreamPlayer.Play( percussionPlayer.GetPlaybackPosition() );
    }

    public override void _Ready()
    {
        audio = this;

        percussionPlayer.Stream = percussion;
        percussionPlayer.Play( MusicTracker.MusicProgress );
        percussionPlayer.Finished += () => percussionPlayer.Play();

        bassPlayer.Stream = bass;
        bassPlayer.Play( MusicTracker.MusicProgress );
        bassPlayer.Finished += () => ResetStream( bassPlayer );

        pianoPlayer.Stream = piano;
        pianoPlayer.Play( MusicTracker.MusicProgress );
        pianoPlayer.Finished += () => ResetStream( pianoPlayer );

        rhythmPlayer.Stream = rhythm;
        rhythmPlayer.Play( MusicTracker.MusicProgress );
        rhythmPlayer.Finished += () => ResetStream( rhythmPlayer );

        MuteRhythm();           
    }




}
