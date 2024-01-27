using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hand : Node2D
{
	[Export]
	public Godot.Collections.Array<Card> cards;

	[Export]
	public Path2D handArc;
	
	[Export]
	public PackedScene cardPrefab;

	void PositionCards() {
		var arcLength = handArc.Curve.GetBakedLength();
		var lengthPerCard =  arcLength/cards.Count;

		for(int i = 0; i < cards.Count; i++){
			var card = cards[i];
			var curvePos = handArc.Curve.SampleBakedWithRotation(i * lengthPerCard);
			card.Transform = curvePos;
			card.Rotate(-Mathf.Pi /2.0f);
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
			var card = cardPrefab.Instantiate<Card>();
			AddChild(card);
			cards.Add(card);
			PositionCards();
		}
	}
}
