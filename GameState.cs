
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

public abstract class Punchline : Playable {
    
    public abstract Damage GetDamage();

    public override GameState PlayCard(GameState game)
    {
        var damage = GetDamage();

        for(int i = 0; i < (int)PatronTag.Count; i ++){
            game.pendingDamage.amounts[i] += damage.amounts[i];
        }
        
        return game;
    }

}


public abstract class Setup : Playable {
    
    public virtual Damage ModifyDamage(Damage d) {return d;}
    public virtual int ModifyATB(int atb) {return atb;}


    public override GameState PlayCard(GameState game)
    {
        game.effectStack.Add(this);
        return game;
    }



}

public struct GameState {




    public List<Setup> effectStack = new List<Setup>();
    public Damage pendingDamage = new Damage();

    public GameState() {}

    // punchline is a mix of positive or negative categories.
    // example setup: times two carnivore, minus one ground.
    // example punchline: one ground, -one carnivore
    // effect: deal negative two carnivore entertainment, zero ground entertainment.


}