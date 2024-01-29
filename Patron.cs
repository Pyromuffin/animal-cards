
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

public partial class Patron : Node2D{


	[Export] public float wiggleRadius, wiggleSpeed;
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

	Vector2 origin;

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

	async void IdleAnimation(){
		await Task.Delay(Random.Shared.Next() % 1000);

		var tween = CreateTween();
		var randomPos = new Vector2(Random.Shared.NextSingle() * wiggleRadius, Random.Shared.NextSingle() * wiggleRadius);
		tween.TweenProperty(this, "position", origin + randomPos, wiggleSpeed);
		tween.TweenCallback(Callable.From(IdleAnimation));
	}


	async void BreatheAnimation(){

		await Task.Delay(Random.Shared.Next() % 1000);

		var tween = CreateTween();
		tween.SetLoops();
		tween.TweenProperty(this, "scale", new Vector2(1.05f, 1.05f), 3f + Random.Shared.NextSingle());
		tween.TweenProperty(this, "scale", new Vector2(1f, 1f), 3f + Random.Shared.NextSingle());
	}


    public override void _Ready() {
		origin = Position;
		IdleAnimation();
		BreatheAnimation();
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
				return new HashSet<PatronTag>{ PatronTag.Carnivore, PatronTag.Ground, PatronTag.Flying, PatronTag.ColdBlooded };

			case PatronType.FROG:
				return new HashSet<PatronTag>{ PatronTag.Carnivore, PatronTag.Ground, PatronTag.Water, PatronTag.ColdBlooded };

			case PatronType.DUCK:
			default:
				return new HashSet<PatronTag>{ PatronTag.Carnivore, PatronTag.Herbivore, PatronTag.Ground, PatronTag.Water, PatronTag.Flying, PatronTag.Bird };
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





}
