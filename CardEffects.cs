

using System.Collections.Generic;
using Godot;

public abstract class Playable{
	public int tableIndex;
	// This is kind of jank.
	public bool inStarterDeck;
	public virtual GameState PlayCard(GameState game) {return game;}
}

public abstract class Punchline : Playable {
	
	public abstract Damage GetDamage();

	public override GameState PlayCard(GameState game)
	{
		Audio.audio.PlayPunchlineSfx();
		Game.game.PlayPunchline( this );
		return game;
	}
}


public abstract class Setup : Playable {
	
	public virtual Damage ModifyDamage(Damage d) {return d;}
	public virtual int ModifyATB(Patron patron, int atb) {return atb;}
	public virtual Damage PostDamageUpdate(Damage d) {return d;}

	public override GameState PlayCard(GameState game)
	{
        Audio.audio.QueueRhythm();
		Audio.audio.PlaySetupSfx();
		game.effectStack.Add(this);
		return game;
	}
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

public class AdditiveDamage : Punchline {

	DamageData[] damageArray;

	public AdditiveDamage ( DamageData[] damageArray )
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

	int drawCards;

	public SetupMultiplierDamage ( DamageData[] damageArray, DamageData[] postDamageMultiplierArray, int drawCards = 0 )
	{
		this.damageArray = damageArray;
		this.postDamageMultiplierArray = postDamageMultiplierArray;
		this.drawCards = drawCards;
	}

	public override GameState PlayCard(GameState game)
	{
		Game.game.extraCardsToDraw += drawCards; 
		return base.PlayCard( game );
	}

	public override Damage ModifyDamage( Damage damage )
	{
		foreach( DamageData damageAndMultiplier in damageArray )
		{
			damage[damageAndMultiplier.tag] += damageAndMultiplier.multiplier;
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


public class SetupAdditiveDamage : Setup {

	DamageData[] damageArray;

	int drawCards;

	public SetupAdditiveDamage ( DamageData[] damageArray, int drawCards = 0 )
	{
		this.damageArray = damageArray;
		this.drawCards = drawCards;
	}

	public override GameState PlayCard(GameState game)
	{
		Game.game.extraCardsToDraw += drawCards; 
		return base.PlayCard( game );
	}

	public override Damage ModifyDamage( Damage damage )
	{
		foreach( DamageData damageAndMultiplier in damageArray )
		{
			damage[damageAndMultiplier.tag] += damageAndMultiplier.multiplier;
		}

		return damage;
	}
}


public class SetupTransform : Setup {

	PatronTag[] fromTags;
	PatronTag toTag;

	public SetupTransform ( PatronTag[] fromTags, PatronTag toTag )
	{
		this.fromTags = fromTags;
		this.toTag = toTag;
	}

	public override Damage PostDamageUpdate(Damage damage)
	{
		int additionalDamage = 0;
		foreach( PatronTag tag in fromTags)
		{
			additionalDamage += damage[tag];
			damage[tag] = 0;
		}
		damage[toTag] += additionalDamage;
		
		return damage;
	}

}

public class SetupPipAttack : Setup {

	HashSet<PatronTag> targetTags;
	int pipChange;

	public SetupPipAttack ( HashSet<PatronTag> targetTags, int pipChange )
	{
		this.targetTags = targetTags;
		this.pipChange = pipChange;
	}

	public override int ModifyATB( Patron patron, int atb ) 
	{
		foreach( PatronTag tag in targetTags)
		{
			if ( patron.tags.Contains( tag ) )
			{
				return System.Math.Max( atb + pipChange, 0 );
			}
		}
		return atb;
	}
}