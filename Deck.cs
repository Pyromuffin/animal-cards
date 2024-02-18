using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


public struct CardData {
	public string name;
	public string description;
	public List<Playable> effect;
}


public partial class Deck : Sprite2D
{
	[Export] public Hand hand;
	[Export] public int initialDeckSize;
	[Export] public PackedScene cardPrefab;

	public Queue<CardData> drawPile = new Queue<CardData>();
	public List<CardData> discardPile = new List<CardData>();

	public void InitializeLevelDeck() {
		drawPile.Clear();
		discardPile.Clear();
		discardPile = new List<CardData>( PlayerData.savedDeck );
	}


	public Task ShuffleDeck() {
		double delay = 0;
		var discardSize = discardPile.Count;

		for(int i = 0; i< discardSize; i++){
			var randomCardIndex = Random.Shared.Next() % discardPile.Count;
			var data = discardPile[randomCardIndex];
			discardPile.RemoveAt(randomCardIndex);
			var card = cardPrefab.Instantiate<Card>();
			GetTree().Root.AddChild(card);
			card.Position = new Vector2(700, 0); // offscreen
			card.Populate(data);
			card.Shuffle(delay);
			delay += 0.05;
		}

		return Task.Delay( (int)(discardSize * 0.05 * 1000) );
	}

	public async void DrawCard(){

		if(drawPile.Count == 0){
			await ShuffleDeck();
		}

		var data = drawPile.Dequeue();
		var card = cardPrefab.Instantiate<Card>();
		card.Populate(data);
		card.hand = hand;
		hand.AddChild(card);
		hand.cards.Add(card);
		hand.PositionCards();
	}


	public async void DrawCards(int count){


		for(int i = 0 ; i < count ; i++){
			if(drawPile.Count == 0){
				Audio.audio.PlayCardShuffleSfx();
				await ShuffleDeck();
				Audio.audio.StopCardShuffleSfx();
			}

			var data = drawPile.Dequeue();
			var card = cardPrefab.Instantiate<Card>();
			card.Populate(data);
			card.hand = hand;
			hand.AddChild(card);
			hand.cards.Add(card);
			card.GlobalPosition = GlobalPosition;
		}
	
		Audio.audio.PlayCardDealSfx();
		hand.PositionCards();
	}
}
