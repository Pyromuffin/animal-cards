
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

    [Export] public Texture2D[] heads;
    [Export] public Texture2D[] bodies;

    [Export] public Sprite2D head, body, eyes, mouth;
    [Export] public Pips pips;

	enum PatronType
	{
		CAT,
		FROG,
		DUCK,
		DRAGON,
		RABBIT,
		SIZE = PatronType.RABBIT
	}
	PatronType patronType;
    [Export] public HealthBar healthBar;

    public override void _Ready() {
        SetupSprites(Random.Shared.Next() % (int)PatronType.SIZE, Random.Shared.Next() % 5);
        pips.Make(maxAtb, startAtb);
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void SetupSprites(int type, int variation) {
		patronType = (PatronType)type;
		tags = GetTags();
        head.Texture = heads[type];
        body.Texture = bodies[variation * 5 + type];
    }


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

	HashSet<PatronTag> GetTags()
	{
		switch(patronType)
		{
			case PatronType.CAT:
				return new HashSet<PatronTag>{ PatronTag.Carnivore, PatronTag.Ground, PatronTag.Mammal };

			case PatronType.RABBIT:
				return new HashSet<PatronTag>{ PatronTag.Herbivore, PatronTag.Ground, PatronTag.Mammal };

			case PatronType.DRAGON:
				return new HashSet<PatronTag>{ PatronTag.Carnivore, PatronTag.Ground, PatronTag.Flying, PatronTag.ColdBlooded };

			case PatronType.FROG:
				return new HashSet<PatronTag>{ PatronTag.Carnivore, PatronTag.Ground, PatronTag.Water, PatronTag.ColdBlooded };

			case PatronType.DUCK:
			default:
				return new HashSet<PatronTag>{ PatronTag.Carnivore, PatronTag.Herbivore, PatronTag.Ground, PatronTag.Water, PatronTag.Flying, PatronTag.Bird };
		}
	}

	public void Kill() {

	}

	public void Attack() {

		Game.game.playerHealth -= attackDamage;

	}

	public override void _Process(double delta)
	{
		//atbLabel.Text = "Heckle: " + currentAtb + " / " + maxAtb;
	}


}
