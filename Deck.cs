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
	public static CardData[] cardData; 
	public static List<List<Playable>> AllCards;


	[Export] public Hand hand;
	[Export] public int initialDeckSize;
	[Export] public PackedScene cardPrefab;

	public Queue<CardData> drawPile = new Queue<CardData>();
	public List<CardData> discardPile = new List<CardData>();

	

 
	public void CreateRandomDeck() {

		drawPile.Clear();
		discardPile.Clear();

		for(int i = 0; i < initialDeckSize; i ++){
			var data = cardData[Random.Shared.Next() % cardData.Length];
			drawPile.Enqueue(data);
		}
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
			delay += 0.1;
		}

		return Task.Delay( (int)(discardSize * 0.1 * 1000) );
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
				GD.Print(discardPile.Count);	
				await ShuffleDeck();
				GD.Print(discardPile.Count);	
			}


			var data = drawPile.Dequeue();
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
		cardData = new CardData[AllCards.Count];

		int index = 0;
		foreach(var card in AllCards){
			var line = lines[card[0].tableIndex - 1];
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

	static void AddEffect(Playable p) {
		var list = new List<Playable>() {p};
		AllCards.Add(list);
	}

	public static void BuildAllCards()
	{
		AllCards = new List<List<Playable>>();


		Playable card = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Water ), new DamageData( PatronTag.Bird ) } );
		card.tableIndex = 3;
		AddEffect( card );

		Playable card2 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Water ), new DamageData( PatronTag.Mammal ) } );
		card2.tableIndex = 4;

		AddEffect( card2 );

		Playable card3 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Flying ), new DamageData( PatronTag.Carnivore ) } );
		card3.tableIndex = 5;

		AddEffect( card3 );

		Playable card4 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Flying ), new DamageData( PatronTag.Bird ) } );
		card4.tableIndex = 6;

		AddEffect( card4 );

		Playable card5 =  new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Herbivore, 2 ) } );
		card5.tableIndex = 7;

		AddEffect( card5 );

		Playable card6 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.ColdBlooded, 2 ) } );
		card6.tableIndex = 8;

		AddEffect( card6 );

		Playable card7 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Ground ), new DamageData( PatronTag.Carnivore ) } );
		card7.tableIndex = 9;

		AddEffect( card7 );

		Playable card8 =  new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Ground, 3 ), new DamageData( PatronTag.Bird, -1 ), new DamageData( PatronTag.ColdBlooded, -1 ) } );
		card8.tableIndex = 10;

		AddEffect( card8 );

		Playable card9 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Herbivore ), new DamageData( PatronTag.Mammal ) } );
		card9.tableIndex = 11;

		AddEffect( card9 );

		Playable card10 = new SetupMultiplierDamage( new DamageData[0], 
			new DamageData[] { new DamageData( PatronTag.Herbivore, 3 ), new DamageData( PatronTag.Carnivore, -3 ) } );
		card10.tableIndex = 13;

		AddEffect( card10 );

		Playable card11 = new SetupMultiplierDamage( new DamageData[] { new DamageData( PatronTag.Water, -3 ) }, 
			new DamageData[] { new DamageData( PatronTag.Mammal, 2 ), new DamageData( PatronTag.ColdBlooded, 2 ) } );
		card11.tableIndex = 15;

		AddEffect( card11 );

		Playable card12 = new SetupMultiplierDamage( new DamageData[] { new DamageData( PatronTag.Flying, -2 ) }, 
			new DamageData[] { new DamageData( PatronTag.Ground, 2 ), } );
		card12.tableIndex = 16;

		AddEffect( card12 );

		Playable card13 = new SetupMultiplierDamage( new DamageData[] { new DamageData( PatronTag.Ground, -1 ), new DamageData( PatronTag.Flying, -1 ) }, 
			new DamageData[0], 3 );
		card13.tableIndex = 12;
		AddEffect( card13 );

		Playable card14 = new SetupMultiplierDamage( new DamageData[] { new DamageData( PatronTag.ColdBlooded, -1 ) }, 
			new DamageData[0], 2 );
		card14.tableIndex = 14;
		AddEffect( card14 );
	}
}
