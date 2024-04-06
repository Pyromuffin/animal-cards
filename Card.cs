using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;


public partial class Card : Sprite2D
{


	[Export] public Texture2D[] icons;
	[Export] public float selectedScale = 1.5f;
	[Export] public float clonkScale = 2.0f;
	[Export] public float slideOutDistance = 10f;
	[Export] public float slideOutTime = 0.3f;
	[Export] public Vector2 playTargetPosition;
	[Export] public Vector2 discardTargetPosition;
	[Export] public float playTime;
	[Export] public Label title;
	[Export] public RichTextLabel description;
	[Export] public Texture2D punchlineTex, setupTex;
	[Export] public CardSelector cardSelector;


	float initialScale;
	public bool selected = false;
	public bool clonked = false;
	public CardData data;
	public Hand hand;

	string InsertImageTags(string desc) {
		var bird = icons[0].ResourcePath;
		var carnivore = icons[1].ResourcePath;
		var coldBlooded = icons[2].ResourcePath;
		var flying = icons[3].ResourcePath;
		var ground = icons[4].ResourcePath;
		var herbivore = icons[5].ResourcePath;
		var mammal = icons[6].ResourcePath;
		var water = icons[7].ResourcePath;
		var dinosaur = icons[8].ResourcePath;

		var replaced = desc.Replace("[B]", "[img width=24 height =24]" + bird + "[/img]");
		replaced = replaced.Replace("[C]", "[img width=24 height =24]" + carnivore + "[/img]");
		replaced = replaced.Replace("[CB]", "[img width=24 height =24]" + coldBlooded + "[/img]");
		replaced = replaced.Replace("[F]", "[img width=24 height =24]" + flying + "[/img]");
		replaced = replaced.Replace("[G]", "[img width=24 height =24]" + ground + "[/img]");
		replaced = replaced.Replace("[H]", "[img width=24 height =24]" + herbivore + "[/img]");
		replaced = replaced.Replace("[M]", "[img width=24 height =24]" + mammal + "[/img]");
		replaced = replaced.Replace("[W]", "[img width=24 height =24]" + water + "[/img]");
		replaced = replaced.Replace("[D]", "[img width=24 height =24]" + dinosaur + "[/img]");
		replaced = replaced.Replace("[N]", "\n");



		return "[color=black]" + replaced + "[/color]";
	}

	public void Populate(CardData data){
		title.Text = data.name;
		description.Text = data.description;
		this.data = data;
		if(data.effect[0] is Punchline) {
			Texture = punchlineTex;
		} else {
			Texture = setupTex;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		initialScale = Scale.X;
		description.Text = InsertImageTags(description.Text);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (clonked) {
			Scale = new Vector2(clonkScale * initialScale, clonkScale * initialScale);
		} else if(selected){
			Scale = new Vector2(selectedScale * initialScale, selectedScale * initialScale);
		} else {
			Scale = new Vector2(initialScale, initialScale);
		}
	}



	public void Shuffle(double delay){

		Game.game.deck.drawPile.Enqueue(data);
		
		var tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
		tween.TweenInterval(delay);
		tween.TweenProperty(this, "position", Game.game.deck.Position, slideOutTime);
		tween.TweenCallback(Callable.From(QueueFree));

		var rotTween = CreateTween();
		rotTween.SetLoops();
		rotTween.TweenInterval(delay);
		rotTween.TweenProperty(this, "rotation", 0, 0.1f);
		rotTween.TweenProperty(this, "rotation", 2 * Mathf.Pi, 0.1f);

	}

	public void FlyoutAnimation() {

		Audio.audio.PlayCardFlyoutSfx();		
		
		var tween = CreateTween();
		Vector2 start = Position;
		Vector2 up = Transform.Y;

		tween.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
		tween.SetParallel(true);
		tween.TweenProperty(this, "position", start + up * slideOutDistance, slideOutTime);
		tween.TweenProperty(this, "rotation", 0f, slideOutTime);
	}



	public void UnflyoutAnimation(double delay) {

		var handPos = hand.GetCardHandTransform(this);
		var rot = handPos.Rotation - Mathf.Pi/2.0f;

		var tween = CreateTween();

		tween.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
		tween.SetParallel(true);
		tween.TweenInterval(delay);
		tween.Chain();
		tween.TweenProperty(this, "position", handPos.Origin, slideOutTime);
		tween.TweenProperty(this, "rotation", rot, slideOutTime);
	}


	void Yeet(){
		hand.cards.Remove(this);
		hand.PositionCards();
		Game.game.deck.discardPile.Add(data);
	}

	void PlayAnimation() {

		Audio.audio.PlayCardSfx();		

		var posTween = CreateTween();
		posTween.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
		posTween.TweenProperty(this, "position", playTargetPosition, playTime);
		posTween.TweenCallback(Callable.From(QueueFree));
		

		var rotTween = CreateTween();
		rotTween.SetLoops();
		rotTween.TweenProperty(this, "rotation", 0, 0.1f);
		rotTween.TweenProperty(this, "rotation", 2 * Mathf.Pi, 0.1f);

	}


	public void DiscardAnimation(double delay) {

		var posTween = CreateTween();
		posTween.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
		posTween.TweenInterval(delay);
		posTween.TweenProperty(this, "position", discardTargetPosition, playTime);
		posTween.TweenCallback(Callable.From(QueueFree));
		
		var rotTween = CreateTween();
		rotTween.SetLoops();
		rotTween.TweenInterval(delay);
		rotTween.TweenProperty(this, "rotation", 0, 0.1f);
		rotTween.TweenProperty(this, "rotation", 2 * Mathf.Pi, 0.1f);

	}

	public void Discard(double delay) {
		Game.game.deck.discardPile.Add(data);
		DiscardAnimation(delay);
	}


	void Play(){
		foreach(var effect in  data.effect){	
		 	effect.PlayCard(Game.game.state);
		}
		Yeet();
		PlayAnimation();
	}

	void _on_area_2d_mouse_entered(){
		if (hand != null && !hand.hoveredCards.Contains(this)) {
			hand.hoveredCards.Insert(0, this);
		}
		else if ( cardSelector != null ) // Hand doesn't exist if we're in the card selection screen
		{
			this.selected = true;
		}
	}

	void _on_area_2d_mouse_exited(){
		if (hand != null) {
			hand.hoveredCards.Remove(this);
		}
		this.selected = false;
		this.ZIndex = 3;
	}


	public void Unclonk() {
		clonked = false;
		UnflyoutAnimation(0);
	}

	void  _on_area_2d_input_event(Node viewPort, InputEvent e, int ShapeIdx){
		if(this.selected && e is InputEventMouseButton butt){
			if(!butt.Pressed)
				return;

			if ( cardSelector != null )
			{
				PlayerData.AddCardToDeck( data );
				cardSelector.NextScene();
				return;
			}

			if(clonked){
				if( data.effect.Count > 0 && data.effect[0] is Punchline && Game.game.state.effectStack.Count == 0 )
				{
					return;
				}
				
				clonked = false;
				Play();
			} else {
				clonked = true;
				foreach(var c in hand.cards){
					if(c != this){
						c.Unclonk();
					}
				}
				FlyoutAnimation();
			}
			
		}

	}

}
