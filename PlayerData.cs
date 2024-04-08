using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerData : Node
{
	public static List<CardData> savedDeck = new List<CardData>();
	public static CardData[] cardData; 
	public static List<List<Playable>> AllCards;
	public static int playerHealth = 10;

	public static int currentLevel = 0;
	public const int MAX_LEVEL = 3;

	public static void CreateStarterDeck() {
		foreach(var card in cardData) {
			if( card.effect[0].inStarterDeck )
			{
				AddCardToDeck( card );
			}
		}
	}

	public static void Reset() {
		savedDeck = new List<CardData>();
		playerHealth = 10;
		currentLevel = 0;
	}

	public static void ParseCardData() {
		if( cardData != null && cardData.Length > 0 )
		{
			return;
		}

		var cardsFile = Godot.FileAccess.Open("res://cards.txt", Godot.FileAccess.ModeFlags.Read);
		string csvText = cardsFile.GetAsText();
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

	public static string GetNextPatronScene()
	{
		switch( currentLevel )
		{
			default:
			case 0:
				return "res://OnePatronScene.tscn";
			case 1:
				return "res://TwoPatronScene.tscn";
			case 2:
				return "res://ThreePatronScene.tscn";
			case 3:
				return "res://FourPatronScene.tscn";
		}
	}

	public static void AddCardToDeck( CardData card )
	{
		savedDeck.Add( card );
	}
	
	static void AddEffect(Playable p) {
		var list = new List<Playable>() {p};
		AllCards.Add(list);
	}

	static void AddEffectList(List<Playable> list) {
		AllCards.Add(list);
	}

	public static void BuildAllCards()
	{
		AllCards = new List<List<Playable>>();

		Playable card = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Water ), new DamageData( PatronTag.Dinosaur, 2 ), new DamageData( PatronTag.Herbivore ) } );
		card.tableIndex = 17;
		AddEffect( card );

		Playable card2 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Water, 2 ), new DamageData( PatronTag.Mammal ), new DamageData( PatronTag.Dinosaur ) } );
		card2.tableIndex = 18;

		AddEffect( card2 );

		Playable card3 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Flying ), new DamageData( PatronTag.Carnivore, 3 ) } );
		card3.tableIndex = 19;

		AddEffect( card3 );

		Playable card4 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Flying ), new DamageData( PatronTag.Dinosaur ), new DamageData( PatronTag.Carnivore ), new DamageData( PatronTag.Ground ) } );
		card4.tableIndex = 20;

		AddEffect( card4 );

		Playable card5 =  new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Herbivore, 2 ), new DamageData( PatronTag.Mammal, 2 ), } );
		card5.tableIndex = 21;

		AddEffect( card5 );

		Playable card6 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Dinosaur, 2 ), new DamageData( PatronTag.Carnivore, 2 ) } );
		card6.tableIndex = 22;

		AddEffect( card6 );

		Playable card7 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Ground, 3 ), new DamageData( PatronTag.Carnivore ) } );
		card7.tableIndex = 23;

		AddEffect( card7 );

		Playable card8 =  new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Ground, 1 ), new DamageData( PatronTag.Dinosaur, 3 ) } );
		card8.tableIndex = 24;

		AddEffect( card8 );

		Playable card9 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Herbivore ), new DamageData( PatronTag.Mammal, 2 ), new DamageData( PatronTag.Water ) } );
		card9.tableIndex = 25;

		AddEffect( card9 );

		Playable card10 = new SetupMultiplierDamage( new DamageData[0], 
			new DamageData[] { new DamageData( PatronTag.Herbivore, 3 ), new DamageData( PatronTag.Carnivore, -3 ) } );
		card10.tableIndex = 13;

		AddEffect( card10 );

		Playable card11 = new SetupMultiplierDamage( new DamageData[] { new DamageData( PatronTag.Water, -3 ) }, 
			new DamageData[] { new DamageData( PatronTag.Mammal, 2 ), new DamageData( PatronTag.Dinosaur, 2 ) } );
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

		Playable card14 = new SetupMultiplierDamage( new DamageData[] { new DamageData( PatronTag.Dinosaur, -1 ) }, 
			new DamageData[0], 2 );
		card14.tableIndex = 14;
		AddEffect( card14 );

		Playable effect1 = new SetupTransform( new PatronTag[] { PatronTag.Mammal }, PatronTag.Dinosaur );
		effect1.tableIndex = 2;

		Playable effect2 = new SetupMultiplierDamage( new DamageData[] { new DamageData( PatronTag.Ground, -2 ) }, new DamageData[0] );
		effect1.tableIndex = 2;
		AddEffectList( new List<Playable>{ effect1, effect2 } );

		Playable card15 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Water ), new DamageData( PatronTag.Bird ) } );
		card15.tableIndex = 3;
		AddEffect( card15 );

		Playable card16 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Water ), new DamageData( PatronTag.Mammal ) } );
		card16.tableIndex = 4;
		AddEffect( card16 );

		Playable card17 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Flying ), new DamageData( PatronTag.Carnivore ) } );
		card17.tableIndex = 5;
		AddEffect( card17 );

		Playable card18 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Flying ), new DamageData( PatronTag.Bird ) } );
		card18.tableIndex = 6;
		AddEffect( card18 );

		Playable card19 =  new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Herbivore, 2 ) } );
		card19.tableIndex = 7;
		AddEffect( card19 );

		Playable card20 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.ColdBlooded, 2 ) } );
		card20.tableIndex = 8;
		AddEffect( card20 );

		Playable card21 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Ground ), new DamageData( PatronTag.Carnivore ) } );
		card21.tableIndex = 9;
		AddEffect( card21 );

		Playable card22 =  new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Ground, 3 ), new DamageData( PatronTag.Bird, -1 ), new DamageData( PatronTag.ColdBlooded, -1 ) } );
		card22.tableIndex = 10;
		AddEffect( card22 );

		Playable card23 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Herbivore ), new DamageData( PatronTag.Mammal ) } );
		card23.tableIndex = 11;
		AddEffect( card23 );

		Playable card24 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Carnivore, 2 ) } );
		card24.tableIndex = 26;
		card24.inStarterDeck = true;
		AddEffect( card24 );

		Playable card25 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Herbivore ), new DamageData( PatronTag.Flying, 2 ) } );
		card25.tableIndex = 27;
		card25.inStarterDeck = true;
		AddEffect( card25 );

		Playable card26 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Ground ), new DamageData( PatronTag.Water, 2 ) } );
		card26.tableIndex = 28;
		card26.inStarterDeck = true;
		AddEffect( card26 );

		Playable card27 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Carnivore ), new DamageData( PatronTag.Water, 2 ) } );
		card27.tableIndex = 29;
		card27.inStarterDeck = true;
		AddEffect( card27 );
		
		Playable card28 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Ground ), new DamageData( PatronTag.Flying, 2 ) } );
		card28.tableIndex = 30;
		card28.inStarterDeck = true;
		AddEffect( card28 );
		
		Playable card29 = new AdditiveDamage( new DamageData[] { new DamageData( PatronTag.Herbivore, 3 ) } );
		card29.tableIndex = 31;
		card29.inStarterDeck = true;
		AddEffect( card29 );

		Playable card30 = new SetupAdditiveDamage( new DamageData[] { new DamageData( PatronTag.Water, -1 ) }, 1 );
		card30.tableIndex = 32;
		card30.inStarterDeck = true;
		AddEffect( card30 );

		Playable card31 = new SetupAdditiveDamage( new DamageData[] { new DamageData( PatronTag.Water, 4 ), new DamageData( PatronTag.Ground, -2 ) }, 1 );
		card31.tableIndex = 33;
		card31.inStarterDeck = true;
		AddEffect( card31 );

		Playable effect3 = new SetupPipAttack( new HashSet<PatronTag> { PatronTag.Carnivore }, -4 );
		effect3.tableIndex = 34;
		effect3.inStarterDeck = true;

		Playable effect4 = new SetupAdditiveDamage( new DamageData[] { new DamageData( PatronTag.Herbivore, -2 ) } );
		effect4.tableIndex = 34;
		effect4.inStarterDeck = true;
		AddEffectList( new List<Playable>{ effect3, effect4 } );

		Playable effect5 = new SetupTransform( new PatronTag[] { PatronTag.Water }, PatronTag.Ground );
		effect5.tableIndex = 35;
		effect5.inStarterDeck = true;

		Playable effect6 = new SetupAdditiveDamage( new DamageData[] { new DamageData( PatronTag.Flying, -1 ) } );
		effect6.tableIndex = 35;
		effect6.inStarterDeck = true;
		AddEffectList( new List<Playable>{ effect5, effect6 } );

		Playable effect7 = new SetupPipAttack( new HashSet<PatronTag> { PatronTag.Flying, PatronTag.Water }, -2 );
		effect7.tableIndex = 36;
		effect7.inStarterDeck = true;

		Playable effect8 = new SetupAdditiveDamage( new DamageData[] { new DamageData( PatronTag.Carnivore, -1 ) } );
		effect8.tableIndex = 36;
		effect8.inStarterDeck = true;
		AddEffectList( new List<Playable>{ effect7, effect8 } );
 
		Playable card32 = new SetupAdditiveDamage( new DamageData[] { new DamageData( PatronTag.Flying, 2 ), new DamageData( PatronTag.Water, 2 ), new DamageData( PatronTag.Ground, -2 ) }, 1 );
		card32.tableIndex = 37;
		card32.inStarterDeck = true;
		AddEffect( card32 );
	}
}
