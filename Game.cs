using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node2D
{

	[Export] public Hand hand;

	public GameState state;

	public List<Patron> patrons;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
