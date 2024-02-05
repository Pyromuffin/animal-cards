using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public partial class Game : Node2D
{

	public static Game game;


	[Export] public int playerHealth = 100;
	[Export] public HealthBar playerHealthBar;

	[Export] public Deck deck;
	[Export] public Label gameStatePreview;
	[Export] public Label healthLabel;
	[Export] public Hand hand;
	public GameState state = new GameState();

	[Export] public Godot.Collections.Array<Patron> patrons;
	[Export] public int cardDrawPerTurn = 5;
	[Export] public Label youWinText, youLoseText;
	public bool won = false;

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
		var cardsFile =Godot.FileAccess.Open("res://cards.txt", Godot.FileAccess.ModeFlags.Read);
		Deck.ParseCardData(cardsFile.GetAsText());
		SecondFrame();
	}

	public async void SecondFrame(){
		await Task.Delay(100);
		deck.CreateRandomDeck();
		StartTurn();
	}


	public void StartTurn(){
		int cardDraw = cardDrawPerTurn + extraCardsToDraw;
		extraCardsToDraw = 0;
		deck.DrawCards(cardDraw);

	}


	public void Win(){
		youWinText.Visible = true;
		hand.Visible = false;
		won = true;
	}

	public void LoseGame(){
		youLoseText.Visible = true;
		hand.Visible = false;
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

		EndTurn();
	}


	public async void EndTurn() {
		// apply game state to patrons.

		var delay = (int) ((hand.cards.Count * hand.positionCardTime) * 1000);
		hand.DiscardHand();
		await Task.Delay(delay);

		bool attacked = false;

		foreach(var p in patrons) {
			p.currentAtb = p.EvaluateATB( state, p.currentAtb );
			
			if(p.currentAtb > p.maxAtb){
				attacked = true;
				p.Attack();
				p.currentAtb = 0;
			}
			p.pips.FillPips(p.currentAtb);
		}
		state = new GameState();

		if(attacked){
			Audio.audio.PlayDamageSfx();
		}


		if(playerHealth <= 0){
			LoseGame();
		}

		StartTurn();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		healthLabel.Text = "Health: " + playerHealth.ToString();
		gameStatePreview.Text = "";
		var previewDamage = GetHypotheticalDamage();

		for(int i = 0; i < (int)PatronTag.Count; i++){
			var tagName = (PatronTag)i;
			gameStatePreview.Text += tagName.ToString() + " : " +  previewDamage.amounts[i] + "\n";
		}
	}


	public void _on_end_turn_button_down(){
		EndTurn();
	}

}
