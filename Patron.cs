
using System;
using System.Collections.Generic;
using Godot;

public enum PatronTag {
		Water,
		Flying,
		Ground,


		Carnivore,
		Herbivore,


		Bird,
		Mammal,
		ColdBlooded,
		Dinosaur,



		Count,
	}

public partial class Patron : Node2D{


	[Export] public Label healthLabel;
	[Export] public Label atbLabel;

	[Export]
	public int maxHealth, minHealth, startHealth;

	public int currentHealth;
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
		SIZE = PatronType.RABBIT + 1
	}

	PatronType patronType;
    [Export] public HealthBar healthBar;

    public override void _Ready() {
        SetupSprites(Random.Shared.Next() % (int)PatronType.SIZE, Random.Shared.Next() % 5);
		int randomATB = Random.Shared.Next() % 3;
		currentAtb = randomATB;
        pips.Make(maxAtb, randomATB);

		int randomHealth = 3 + Random.Shared.Next() % 8;
		currentHealth = randomHealth;
        healthBar.SetHealth(randomHealth, maxHealth);
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

		if( currentHealth < maxHealth / 2 )
			atb++;
			
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
				return new HashSet<PatronTag>{ PatronTag.Carnivore, PatronTag.Ground, PatronTag.Flying, PatronTag.ColdBlooded, PatronTag.Dinosaur };

			case PatronType.FROG:
				return new HashSet<PatronTag>{ PatronTag.Carnivore, PatronTag.Ground, PatronTag.Water, PatronTag.ColdBlooded, PatronTag.Dinosaur };

			case PatronType.DUCK:
			default:
				return new HashSet<PatronTag>{ PatronTag.Carnivore, PatronTag.Herbivore, PatronTag.Ground, PatronTag.Water, PatronTag.Flying, PatronTag.Bird, PatronTag.Dinosaur };
		}
	}

    void Yeet(){
        Game.game.patrons.Remove(this);
        if(Game.game.patrons.Count == 0) {
            Game.game.Win();
        }
        QueueFree();
    }

	public void Kill() {


        var posTween = CreateTween();
		posTween.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
        var offScreen = new Vector2(320, -700);
		posTween.TweenProperty(this, "position", offScreen, 1.0);
		posTween.TweenCallback(Callable.From(Yeet));
		
		var rotTween = CreateTween();
		rotTween.SetLoops();
		rotTween.TweenProperty(this, "rotation", 0, 0.1f);
		rotTween.TweenProperty(this, "rotation", 2 * Mathf.Pi, 0.1f);
	}

	public void Attack() {

		Game.game.playerHealth -= attackDamage;

	}

	public override void _Process(double delta)
	{
		//atbLabel.Text = "Heckle: " + currentAtb + " / " + maxAtb;
	}


}
