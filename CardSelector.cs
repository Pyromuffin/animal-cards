using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public partial class CardSelector : Node
{
	public enum CardSelectorState
	{
		SETUP,
		PUNCHLINE,
	}
	
	public static CardSelectorState State = CardSelectorState.SETUP;
	[Export] public Card[] cardPrefabs;
	[Export] public ChangeScene changeScene;


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
		if ( State == CardSelectorState.SETUP )
		{
			SpawnCards( true, false );
		}
		else
		{
			SpawnCards( false, true );
		}
	}

	public void SpawnCards( bool forceSetUp, bool forcePunchline )
	{
		List<CardData> cards = GetCards( cardPrefabs.Length, forceSetUp, forcePunchline );
		for( int cardIndex = 0; cardIndex < cardPrefabs.Length; cardIndex++ )
		{
			cardPrefabs[cardIndex].Populate(cards[cardIndex]);
			cardPrefabs[cardIndex].Visible = true;
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

	public void NextScene()
	{
		switch( CardSelector.State )
		{
			case CardSelector.CardSelectorState.SETUP:
				State = CardSelector.CardSelectorState.PUNCHLINE;
				changeScene.ChangeToScene( "res://CardSelect.tscn" );
				break;
			default:
			case CardSelector.CardSelectorState.PUNCHLINE:
				State = CardSelector.CardSelectorState.SETUP;
				changeScene.ChangeToGame();
				break;
		}
	}
}
