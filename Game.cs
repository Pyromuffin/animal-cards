using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node2D
{

	public static Game game;


	public int playerHealth = 100;


	[Export] public Label gameStatePreview;
	[Export] public Label healthLabel;
	[Export] public Hand hand;
	public GameState state = new GameState();

	[Export] public Godot.Collections.Array<Patron> patrons;
	[Export] public int cardDrawPerTurn = 5;


	class Hypothetical : Punchline {
		public override Damage GetDamage()
		{
			var d = new Damage();
			for(int i = 0; i < (int)PatronTag.Count; i++){
				d.amounts[i] = 1;
			}
			return d;
		}
	}

	public Damage GetHypotheticalDamage(){
		return state.GetDamageForPunchline(new Hypothetical());
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		game = this;
	}


	public void StartTurn(){
		// deal cards

	}


	public void PlayPunchline(Punchline punch) {
		// also ends turn;

		foreach(var p in patrons) {
			p.currentHealth -= p.EvaluateDamage(state, punch);
			if(p.currentHealth >= p.maxHealth){
				p.Kill();
			}
		}

		EndTurn();
	}


	public void EndTurn() {
		// apply game state to patrons.

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
		gameStatePreview.Text = "";
		var previewDamage = GetHypotheticalDamage();

		for(int i = 0; i < (int)PatronTag.Count; i++){
			var tagName = (PatronTag)i;
			gameStatePreview.Text += tagName.ToString() + " : " +  previewDamage.amounts[i] + "\n";
		}
	}


	public void _on_end_turn_button_down(){
		EndTurn();
	}

}
