
using System;
using System.Collections.Generic;
using Godot;

public partial class Patron : Node2D{


	[Export] public Label healthLabel;
	[Export] public Label atbLabel;

	[Export]
	public int maxHealth, minHealth, currentHealth, startHealth;
	[Export]
	public int maxAtb, currentAtb, startAtb;
	[Export]
	public int attackDamage;

	public HashSet<PatronTag> tags = new HashSet<PatronTag>();

	public int EvaluateDamage(GameState state, Punchline p) {

		var damage = 0;

		foreach(var tag in tags){
			damage += state.GetDamageForPunchline(p)[tag];
		}

		return damage;
	}


   public int EvaluateATB( GameState state) {
		
		var atb = 1;

		foreach(var e in state.effectStack) {
			atb = e.ModifyATB(atb);
		}

		return atb;
	}

	public void Kill() {

	}

	public void Attack() {

		Game.game.playerHealth -= attackDamage;

	}

	public override void _Process(double delta)
	{
		healthLabel.Text = "Entertainment: " + currentHealth + " / " + maxHealth;
		atbLabel.Text = "Heckle: " + currentAtb + " / " + maxAtb;
	}


}
