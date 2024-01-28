using Godot;
using System;
using System.Collections.Generic;

public partial class Deck : Node
{
	public static List<Playable> AllCards;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void BuildAllCards()
	{
		AllCards = new List<Playable>();
		Playable card = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Water ), new DamageData( PatronTag.Bird ) } );
		AllCards.Add( card );

		Playable card2 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Water ), new DamageData( PatronTag.Mammal ) } );
		AllCards.Add( card2 );

		Playable card3 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Flying ), new DamageData( PatronTag.Carnivore ) } );
		AllCards.Add( card3 );

		Playable card4 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Flying ), new DamageData( PatronTag.Bird ) } );
		AllCards.Add( card4 );

		Playable card5 =  new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Herbivore, 2 ) } );
		AllCards.Add( card5 );

		Playable card6 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.ColdBlooded, 2 ) } );
		AllCards.Add( card6 );

		Playable card7 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Ground ), new DamageData( PatronTag.Carnivore ) } );
		AllCards.Add( card7 );

		Playable card8 =  new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Ground, 3 ), new DamageData( PatronTag.Bird, -1 ), new DamageData( PatronTag.ColdBlooded, -1 ) } );
		AllCards.Add( card8 );

		Playable card9 = new MultiplierDamage( new DamageData[] { new DamageData( PatronTag.Herbivore ), new DamageData( PatronTag.Mammal ) } );
		AllCards.Add( card9 );

		Playable card10 = new SetupMultiplierDamage( new DamageData[0], 
			new DamageData[] { new DamageData( PatronTag.Herbivore, 3 ), new DamageData( PatronTag.Carnivore, -3 ) } );
		AllCards.Add( card10 );

		Playable card11 = new SetupMultiplierDamage( new DamageData[] { new DamageData( PatronTag.Water, -3 ) }, 
			new DamageData[] { new DamageData( PatronTag.Mammal, 2 ), new DamageData( PatronTag.ColdBlooded, 2 ) } );
		AllCards.Add( card11 );

		Playable card12 = new SetupMultiplierDamage( new DamageData[] { new DamageData( PatronTag.Flying, -2 ) }, 
			new DamageData[] { new DamageData( PatronTag.Ground, 2 ), } );
		AllCards.Add( card12 );
	}
}
