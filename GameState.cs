
using System;
using System.Collections.Generic;
using Godot;

public enum PatronTag {
	Water,
	Flying,
	Ground,


	Carnivore,
	Herbivore,


	Bird,
	Mammal,
	ColdBlooded,



	Count,
}


public struct Damage{
	public int[] amounts = new int[(int)PatronTag.Count];

	public Damage(){}

	public int this[PatronTag c]
	{
		get { return amounts[(int)c]; }
		set { amounts[(int)c] = value; }
	}

}

public struct GameState {


	public List<Setup> effectStack = new List<Setup>();

	public Damage GetDamageForPunchline(Punchline p) {
		var d = p.GetDamage();
		foreach(var s in effectStack) {
			d = s.ModifyDamage(d);
		}

		foreach(var s in effectStack) {
			d = s.PostDamageUpdate(d);
		}

		return d;
	}

	public GameState() {}

	// punchline is a mix of positive or negative categories.
	// example setup: times two carnivore, minus one ground.
	// example punchline: one ground, -one carnivore
	// effect: deal negative two carnivore entertainment, zero ground entertainment.


}
