using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public partial class CardSelector : Node
{
	[Export] public Card[] cardPrefabs;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayerData.ParseCardData();
		SecondFrame();
	}

	public async void SecondFrame(){
		await Task.Delay(100);
		if( PlayerData.savedDeck.Count == 0 )
		{
			PlayerData.CreateStarterDeck();
		}
		SpawnCards( true, false );
	}

	public void SpawnCards( bool forceSetUp, bool forcePunchline )
	{
		List<CardData> cards = GetCards( cardPrefabs.Length, forceSetUp, forcePunchline );
		for( int cardIndex = 0; cardIndex < cardPrefabs.Length; cardIndex++ )
		{
			cardPrefabs[cardIndex].Populate(cards[cardIndex]);
		}
	}

	public List<CardData> GetCards( int count, bool forceSetUp, bool forcePunchline )
	{
		List<CardData> cards = new List<CardData>();
		while( cards.Count < count )
		{
			int randomIndex = Random.Shared.Next() % PlayerData.cardData.Length;
			CardData chosenCard = PlayerData.cardData[randomIndex];
			if ( !chosenCard.effect[0].inStarterDeck && ( !forceSetUp || chosenCard.effect[0] is Setup ) 
				&& ( !forcePunchline || chosenCard.effect[0] is Punchline )
				&& !cards.Contains( chosenCard ) )
			{
				cards.Add( chosenCard );
			}
		}

		return cards;
	}
}
