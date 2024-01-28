using Godot;
using System;
using System.Threading.Tasks;


public partial class Card : Sprite2D
{


	[Export] public float selectedScale = 1.5f;
	[Export] public float clonkScale = 2.0f;
	[Export] public float slideOutDistance = 10f;
	[Export] public float slideOutTime = 0.3f;
	[Export] public Vector2 playTargetPosition;
	[Export] public Vector2 discardTargetPosition;
	[Export] public float playTime;
	[Export] public Label title;
	[Export] public Label description;
	[Export] public Texture2D punchlineTex, setupTex;

	float initialScale;
	public Playable cardEffect;
	public bool selected = false;
	public bool clonked = false;
	public CardData data;
	public Hand hand;

	public void Populate(CardData data){
		title.Text = data.name;
		description.Text = data.description;
		cardEffect = data.effect;
		this.data = data;
		if(data.effect is Punchline) {
			Texture = punchlineTex;
		} else {
			Texture = setupTex;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		initialScale = Scale.X;
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
		cardEffect.PlayCard(Game.game.state);
		Yeet();
		PlayAnimation();
	}

	void _on_area_2d_mouse_entered(){
		if (hand != null && !hand.hoveredCards.Contains(this)) {
			hand.hoveredCards.Insert(0, this);
		}
	}

	void _on_area_2d_mouse_exited(){
		if (hand != null) {
			hand.hoveredCards.Remove(this);
		}
		this.selected = false;
		this.ZIndex = 3;
	}

	void  _on_area_2d_input_event(Node viewPort, InputEvent e, int ShapeIdx){
		if(this.selected && e is InputEventMouseButton butt){
			if(!butt.Pressed)
				return;

			if(clonked){
				clonked = false;
				Play();
			} else {
				clonked = true;
				FlyoutAnimation();
			}
			
		}

	}

}
