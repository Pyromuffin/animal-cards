using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public partial class Game : Node2D
{

	public static Game game;


	[Export] public HealthBar playerHealthBar;

	[Export] public Deck deck;
	[Export] public Label gameStatePreview;
	[Export] public Label healthLabel;
	[Export] public Hand hand;
	public GameState state = new GameState();

	[Export] public Godot.Collections.Array<Patron> patrons;
	[Export] public int cardDrawPerTurn = 5;
	[Export] public Label youWinText, youLoseText;
	[Export] public ChangeScene changeSceneNode;
	public bool won = false;
	public static bool DebugPrint = false;

	int MAX_PLAYER_HEALTH = 6;

	bool endingTurn = false;


	public int extraCardsToDraw = 0;

	class Hypothetical : Punchline {
		public override Damage GetDamage()
		{
			var d = new Damage();
			for(int i = 0; i < (int)PatronTag.Count; i++){
				d.amounts[i] = 1;
			}
			return d;
		}
	}

	public Damage GetHypotheticalDamage(){
		return state.GetDamageForPunchline(new Hypothetical());
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		game = this;
		
		PlayerData.ParseCardData();
		game.SetPlayerHealth(PlayerData.playerHealth);
		SecondFrame();
	}

	public async void SecondFrame(){
		await Task.Delay(100);
		if( PlayerData.savedDeck.Count == 0 )
		{
			PlayerData.CreateStarterDeck();
		}
		deck.InitializeLevelDeck();
		StartTurn();
	}


	public void SetPlayerHealth( int health )
	{
		Game.game.playerHealthBar.SetHealth( health, MAX_PLAYER_HEALTH, false );
	}


	public void StartTurn()
	{
		int cardDraw = cardDrawPerTurn + extraCardsToDraw;
		extraCardsToDraw = 0;
		deck.DrawCards(cardDraw);

	}


	public void Win(){
		PlayerData.currentLevel++;
		if(PlayerData.currentLevel > PlayerData.MAX_LEVEL )
		{
			changeSceneNode.ChangeToScene( "res://WinScene.tscn" );
		}
		else
		{
			changeSceneNode.ChangeToScene( "res://CardSelect.tscn" );
		}
	}

	public void LoseGame(){
		changeSceneNode.ChangeToScene( "res://LoseScene.tscn" );
	}

	public void PlayPunchline(Punchline punch) {
		// also ends turn;
		Audio.audio.PlayPunchline();

		foreach(var p in patrons) {
			p.currentHealth += p.EvaluateDamage(state, punch);
			p.healthBar.SetHealth(p.currentHealth, p.maxHealth);
			if(p.currentHealth >= p.maxHealth){
				p.Kill();
			}
		}

		EndTurn( true );
		state = new GameState();
	}


	public static void ReplaceText( RichTextLabel label )
	{
		var bird = "res://art/ui/icons/icon_bird.png";
		var carnivore = "res://art/ui/icons/icon_carnivore.png";
		var coldBlooded = "res://art/ui/icons/icon_coldblooded.png";
		var flying = "res://art/ui/icons/icon_flying.png";
		var ground = "res://art/ui/icons/icon_ground.png";
		var herbivore = "res://art/ui/icons/icon_herbivore.png";
		var mammal = "res://art/ui/icons/icon_mammal.png";
		var water = "res://art/ui/icons/icon_water.png";
		var dinosaur = "res://art/ui/icons/icon_dinosaur.png";

		var replaced = label.Text.Replace("[B]", "[img width=24 height =24]" + bird + "[/img]");
		replaced = replaced.Replace("[C]", "[img width=24 height =24]" + carnivore + "[/img]");
		replaced = replaced.Replace("[CB]", "[img width=24 height =24]" + coldBlooded + "[/img]");
		replaced = replaced.Replace("[F]", "[img width=24 height =24]" + flying + "[/img]");
		replaced = replaced.Replace("[G]", "[img width=24 height =24]" + ground + "[/img]");
		replaced = replaced.Replace("[H]", "[img width=24 height =24]" + herbivore + "[/img]");
		replaced = replaced.Replace("[M]", "[img width=24 height =24]" + mammal + "[/img]");
		replaced = replaced.Replace("[W]", "[img width=24 height =24]" + water + "[/img]");
		replaced = replaced.Replace("[D]", "[img width=24 height =24]" + dinosaur + "[/img]");

		label.Text = replaced;
	}


	public async void EndTurn( bool playedPunchline ) {
		if( endingTurn )
			return;
		endingTurn = true;
		bool attacked = false;

		foreach(var p in patrons) {
			p.currentAtb = p.EvaluateATB( state, p.currentAtb, playedPunchline );
			
			if(p.currentAtb > p.maxAtb){
				attacked = true;
				p.Attack();
				p.currentAtb = 0;
			}
			p.pips.FillPips(p.currentAtb);
		}
			
		// apply game state to patrons.

		var delay = (int) ((hand.cards.Count * hand.positionCardTime) * 1000);
		hand.DiscardHand();
		await Task.Delay(delay);

		if(attacked){
			Audio.audio.PlayDamageSfx();
		}


		if(PlayerData.playerHealth <= 0){
			LoseGame();
		}

		StartTurn();
		endingTurn = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		healthLabel.Text = "Health: " + PlayerData.playerHealth.ToString();
		gameStatePreview.Text = "";
		var previewDamage = GetHypotheticalDamage();

		for(int i = 0; i < (int)PatronTag.Count; i++){
			var tagName = (PatronTag)i;
			gameStatePreview.Text += tagName.ToString() + " : " +  previewDamage.amounts[i] + "\n";
		}
	}


	public void _on_end_turn_button_down(){
		EndTurn( false );
	}

}
