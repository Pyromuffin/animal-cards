using Godot;
using System;
using System.Collections.Generic;


	public struct CardData {
		public string name;
		public string description;
		public Playable effect;
	}


public partial class Deck : Sprite2D
{
	public static CardData[] cardData; 
	public static List<Playable> AllCards;


	[Export] public Hand hand;
	[Export] public int initialDeckSize;
	[Export] public PackedScene cardPrefab;

	public Queue<int> drawPile = new Queue<int>();
	public Queue<int> discardPile = new Queue<int>();


	public void CreateRandomDeck() {

		drawPile.Clear();
		discardPile.Clear();

		for(int i = 0; i < cardData.Length; i ++){
			drawPile.Enqueue(Random.Shared.Next() % cardData.Length);
		}
	}


	public void DrawCard(){
		var data = cardData[drawPile.Dequeue()];
		var card = cardPrefab.Instantiate<Card>();
		card.Populate(data);
		card.hand = hand;
		hand.AddChild(card);
		hand.cards.Add(card);
		hand.PositionCards();
	}


	public void DrawCards(int count){
		for(int i = 0 ; i < count ; i++){
			var data = cardData[drawPile.Dequeue()];
			var card = cardPrefab.Instantiate<Card>();
			card.Populate(data);
			card.hand = hand;
			hand.AddChild(card);
			hand.cards.Add(card);
			card.GlobalPosition = GlobalPosition;
		}
	
		hand.PositionCards();
	}

	

	public static void ParseCardData(string csvText) {

		BuildAllCards();

		var lines = csvText.Split("\n");
		cardData = new CardData[lines.Length];

		int index = 0;
		foreach(var card in AllCards){
			var line = lines[card.tableIndex - 1];
			var fields = line.Split(",");
			var name = fields[1];
			var desc = fields[2];

			var data = new CardData();
			data.name = name;
			data.description = desc;
			data.effect = card;
			cardData[index] = data;
			index++;
		}

	}


	public static void BuildAllCards()
	{
		AllCards = new List<Playable>();
		Playable card = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Water ), new DamageData( PatronTag.Bird ) } );
		card.tableIndex = 3;
		AllCards.Add( card );

		Playable card2 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Water ), new DamageData( PatronTag.Mammal ) } );
		card2.tableIndex = 4;
		AllCards.Add( card2 );

		Playable card3 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Flying ), new DamageData( PatronTag.Carnivore ) } );
		card3.tableIndex = 5;
		AllCards.Add( card3 );

		Playable card4 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Flying ), new DamageData( PatronTag.Bird ) } );
		card4.tableIndex = 6;
		AllCards.Add( card4 );

		Playable card5 =  new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Herbivore, 2 ) } );
		card5.tableIndex = 7;
		AllCards.Add( card5 );

		Playable card6 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.ColdBlooded, 2 ) } );
		card6.tableIndex = 8;
		AllCards.Add( card6 );

		Playable card7 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Ground ), new DamageData( PatronTag.Carnivore ) } );
		card7.tableIndex = 9;
		AllCards.Add( card7 );

		Playable card8 =  new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Ground, 3 ), new DamageData( PatronTag.Bird, -1 ), new DamageData( PatronTag.ColdBlooded, -1 ) } );
		card8.tableIndex = 10;
		AllCards.Add( card8 );

		Playable card9 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Herbivore ), new DamageData( PatronTag.Mammal ) } );
		card9.tableIndex = 11;
		AllCards.Add( card9 );

		Playable card10 = new SetupMultiplierDamage( new DamageData[0], 
			new DamageData[] { new DamageData( PatronTag.Herbivore, 3 ), new DamageData( PatronTag.Carnivore, -3 ) } );
		card10.tableIndex = 13;
		AllCards.Add( card10 );

		Playable card11 = new SetupMultiplierDamage( new DamageData[] { new DamageData( PatronTag.Water, -3 ) }, 
			new DamageData[] { new DamageData( PatronTag.Mammal, 2 ), new DamageData( PatronTag.ColdBlooded, 2 ) } );
		card11.tableIndex = 15;
		AllCards.Add( card11 );

		Playable card12 = new SetupMultiplierDamage( new DamageData[] { new DamageData( PatronTag.Flying, -2 ) }, 
			new DamageData[] { new DamageData( PatronTag.Ground, 2 ), } );
		card12.tableIndex = 16;
		AllCards.Add( card12 );
	}
}
