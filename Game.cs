using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node2D
{

	public static Game game;


	public int playerHealth = 100;

	[Export] public Label healthLabel;

	[Export] public Hand hand;

	public GameState state = new GameState();

	[Export]
	public Godot.Collections.Array<Patron> patrons;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		game = this;
	}


	public void EndTurn() {
		// apply game state to patrons.

		// apply post damage effects
		foreach(var e in state.effectStack){
			state = e.PostDamageUpdate(state);
		}

		foreach(var p in patrons) {
			p.currentHealth += p.EvaluateDamage(state);
			if(p.currentHealth >= p.maxHealth){
				p.Kill();
			}
		}

		foreach(var p in patrons) {
			p.currentAtb += p.EvaluateATB(state);
			if(p.currentAtb >= p.maxAtb){
				p.Attack();
				p.currentAtb = 0;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		healthLabel.Text = "Health: " + playerHealth.ToString();
	}


	public void _on_end_turn_button_down(){
		EndTurn();
	}

}
