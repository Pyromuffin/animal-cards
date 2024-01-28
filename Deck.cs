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
		Playable card = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Water ), new DamageAndMultiplier( PatronTag.Bird ) } );
		AllCards.Add( card );

		Playable card2 = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Water ), new DamageAndMultiplier( PatronTag.Mammal ) } );
		AllCards.Add( card2 );

		Playable card3 = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Flying ), new DamageAndMultiplier( PatronTag.Carnivore ) } );
		AllCards.Add( card3 );

		Playable card4 = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Flying ), new DamageAndMultiplier( PatronTag.Bird ) } );
		AllCards.Add( card4 );

		Playable card5 =  new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Herbivore, 2 ) } );
		AllCards.Add( card5 );

		Playable card6 = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.ColdBlooded, 2 ) } );
		AllCards.Add( card6 );

		Playable card7 = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Ground ), new DamageAndMultiplier( PatronTag.Carnivore ) } );
		AllCards.Add( card7 );

		Playable card8 =  new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Ground, 3 ), new DamageAndMultiplier( PatronTag.Bird, -1 ), new DamageAndMultiplier( PatronTag.ColdBlooded, -1 ) } );
		AllCards.Add( card8 );

		Playable card9 = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Herbivore ), new DamageAndMultiplier( PatronTag.Mammal ) } );
		AllCards.Add( card9 );
	}
}
