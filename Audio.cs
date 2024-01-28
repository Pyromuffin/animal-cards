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


    [Export] AudioStreamMP3 percussion, bass, piano, rhythm;

    [Export] AudioStreamPlayer player;


    public override void _Ready()
    {
        player.Stream = percussion;
        player.Play();
    }




}