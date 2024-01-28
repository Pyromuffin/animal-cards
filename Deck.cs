using Godot;
using System;
using System.Collections.Generic;

public partial class Deck : Node
{
	public static List<Card> AllCards;
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
		AllCards = new List<Card>();
		Card card = new Card();
		card.cardEffect = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Water ), new DamageAndMultiplier( PatronTag.Bird ) } );
		AllCards.Add( card );

		Card card2 = new Card();
		card2.cardEffect = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Water ), new DamageAndMultiplier( PatronTag.Mammal ) } );
		AllCards.Add( card2 );

		Card card3 = new Card();
		card3.cardEffect = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Flying ), new DamageAndMultiplier( PatronTag.Carnivore ) } );
		AllCards.Add( card3 );

		Card card4 = new Card();
		card4.cardEffect = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Flying ), new DamageAndMultiplier( PatronTag.Bird ) } );
		AllCards.Add( card4 );

		Card card5 = new Card();
		card5.cardEffect = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Herbivore, 2 ) } );
		AllCards.Add( card5 );

		Card card6 = new Card();
		card6.cardEffect = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.ColdBlooded, 2 ) } );
		AllCards.Add( card6 );

		Card card7 = new Card();
		card7.cardEffect = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Ground ), new DamageAndMultiplier( PatronTag.Carnivore ) } );
		AllCards.Add( card7 );

		Card card8 = new Card();
		card8.cardEffect = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Ground, 3 ), new DamageAndMultiplier( PatronTag.Bird, -1 ), new DamageAndMultiplier( PatronTag.ColdBlooded, -1 ) } );
		AllCards.Add( card8 );

		Card card9 = new Card();
		card9.cardEffect = new MultiplierDamage( new DamageAndMultiplier[] { new DamageAndMultiplier( PatronTag.Herbivore ), new DamageAndMultiplier( PatronTag.Mammal ) } );
		AllCards.Add( card9 );
	}
}
