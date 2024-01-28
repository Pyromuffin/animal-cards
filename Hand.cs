using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hand : Node2D
{
	[Export]
	public Godot.Collections.Array<Card> cards;

	[Export]
	public float cardScale;

	[Export]
	public Path2D handArc;
	
	[Export] double positionCardTime = 0.1;
	
	[Export]
	public Godot.Collections.Array<Card> hoveredCards;


	public Transform2D GetCardHandTransform(Card c) {
		var index = cards.IndexOf(c);

		var arcLength = handArc.Curve.GetBakedLength();
		var lengthPerCard =  arcLength/cards.Count;
		var curvePos = handArc.Curve.SampleBakedWithRotation( (index + 1) * lengthPerCard );

		return curvePos;
	}

	public void PositionCards() {
		double delay = 0;

		foreach(var c in cards) {
			c.UnflyoutAnimation(delay);
			delay += positionCardTime;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PositionCards();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsKeyPressed(Key.Space)) {
			PositionCards();
		}
		
		for(int i = 0; i < hoveredCards.Count; i++){
			var card = hoveredCards[i];
			var isSelected = i == 0;
			card.selected = isSelected;
			card.ZIndex = isSelected ? 1 : 0;
		}
	}
}
