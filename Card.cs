using Godot;
using System;


public partial class Card : Sprite2D
{
	public Playable cardEffect;

	public bool selected = false;
	public bool clonked = false;
	[Export] public float selectedScale = 1.5f;
	[Export] public float clonkScale = 2.0f;
	[Export] public float slideOutDistance = 10f;
	[Export] public float slideOutTime = 0.3f;
	[Export] public Vector2 playTargetPosition;
	[Export] public float playTime;
	float initialScale;

	public Hand hand;

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


	void FlyoutAnimation() {
		var tween = CreateTween();

		Vector2 start = Position;
		Vector2 up = Transform.Y;

		tween.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
		tween.SetParallel(true);
		tween.TweenProperty(this, "position", start + up * slideOutDistance, slideOutTime);
		tween.TweenProperty(this, "rotation", 0f, slideOutTime);
	}



	void UnflyoutAnimation() {

		var handPos = hand.GetCardHandTransform(this);
		var rot = handPos.Rotation - Mathf.Pi/2.0f;

		var tween = CreateTween();

		tween.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
		tween.SetParallel(true);
		tween.TweenProperty(this, "position", handPos.Origin, slideOutTime);
		tween.TweenProperty(this, "rotation", rot, slideOutTime);
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


	void Play(){
		cardEffect.PlayCard();
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
		this.ZIndex = 0;
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
