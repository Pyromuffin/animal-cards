

public abstract class Playable{

	public virtual GameState PlayCard(GameState game) {return game;}

}


public class OnlyBirdsAreReal : Setup {

	// Total cold blooded damage is added to bird damage
	// Total mammal damage is added to bird damage

	public override Damage PostDamageUpdate(Damage d)
	{
		var birdDamage = d[PatronTag.Bird];
		birdDamage += d[PatronTag.Mammal];
		birdDamage += d[PatronTag.ColdBlooded];
		
		d[PatronTag.Bird] = birdDamage;

		d[PatronTag.Mammal] = 0;
		d[PatronTag.ColdBlooded] = 0;
		
		return d;
	}

}


public class BirdsAreNotReal : Setup {

	public override Damage PostDamageUpdate(Damage d) { 
		d[PatronTag.Bird] *= 2;
		return d;
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

public struct DamageData
{
	public PatronTag tag;
	public int multiplier;
	public DamageData( PatronTag tag, int multiplier = 1 )
	{
		this.tag = tag;
		this.multiplier = multiplier;
	}
}

public class MultiplierDamage : Punchline {

	DamageData[] damageArray;

	public MultiplierDamage ( DamageData[] damageArray )
	{
		this.damageArray = damageArray;
	}

	public override Damage GetDamage()
	{
		var damage = new Damage();
		foreach( DamageData damageAndMultiplier in damageArray )
		{
			damage[damageAndMultiplier.tag] = damageAndMultiplier.multiplier;
		}

		return damage;
	}
}

public class SetupMultiplierDamage : Setup {

	DamageData[] damageArray;
	DamageData[] postDamageMultiplierArray;

	public SetupMultiplierDamage ( DamageData[] damageArray, DamageData[] postDamageMultiplierArray )
	{
		this.damageArray = damageArray;
		this.postDamageMultiplierArray = postDamageMultiplierArray;
	}

	public override Damage ModifyDamage( Damage damage )
	{
		foreach( DamageData damageAndMultiplier in damageArray )
		{
			damage[damageAndMultiplier.tag] = damageAndMultiplier.multiplier;
		}

		return damage;
	}

	public override Damage PostDamageUpdate(Damage damage) { 
		foreach( DamageData damageAndMultiplier in postDamageMultiplierArray )
		{
			damage[damageAndMultiplier.tag] *= damageAndMultiplier.multiplier;
		}
		return damage;
	}
}


