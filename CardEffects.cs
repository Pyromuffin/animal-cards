

public abstract class Playable{

	public virtual GameState PlayCard(GameState game) {return game;}
	public virtual GameState PostDamageUpdate(GameState game) {return game;}

}


public class OnlyBirdsAreReal : Setup {

	// Total cold blooded damage is added to bird damage
	// Total mammal damage is added to bird damage

	public override GameState PostDamageUpdate(GameState game)
	{
		var birdDamage = game.pendingDamage[PatronTag.Bird];
		birdDamage += game.pendingDamage[PatronTag.Mammal];
		birdDamage += game.pendingDamage[PatronTag.ColdBlooded];
		
		game.pendingDamage[PatronTag.Bird] = birdDamage;

		game.pendingDamage[PatronTag.Mammal] = 0;
		game.pendingDamage[PatronTag.ColdBlooded] = 0;
		
		return game;
	}

}


public class BirdsAreNotReal : Setup {

	public override GameState PostDamageUpdate(GameState game) { 
		game.pendingDamage[PatronTag.Bird] *= 2;
		return game;
	}

}


public class RoastChicken : Punchline {

	public override Damage GetDamage()
	{
		var damage = new Damage();
		damage[PatronTag.Bird] = 1;

		return damage;
	}
}

public struct DamageAndMultiplier
{
	public PatronTag tag;
	public int multiplier;
	public DamageAndMultiplier( PatronTag tag, int multiplier = 1 )
	{
		this.tag = tag;
		this.multiplier = multiplier;
	}
}

public class MultiplierDamage : Punchline {

	DamageAndMultiplier[] damageArray;

	public MultiplierDamage ( DamageAndMultiplier[] damageArray )
	{
		this.damageArray = damageArray;
	}

	public override Damage GetDamage()
	{
		var damage = new Damage();
		foreach( DamageAndMultiplier damageAndMultiplier in damageArray )
		{
			damage[damageAndMultiplier.tag] = damageAndMultiplier.multiplier;
		}

		return damage;
	}
}

