
using System;
using System.Collections.Generic;
using Godot;

public enum Category {
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

public class BirdsAreNotReal : Setup {

    public override Damage ModifyDamage(Damage d) { 
        d[Category.Bird] *= 2;
        return d;
    }

}


public class RoastChicken : Punchline {

    public override Damage GetDamage()
    {
        var damage = new Damage();
        damage[Category.Bird] = 1;

        return damage;
    }

}


public class Patron {

    public int maxHealth, minHealth, currentHealth;
    public int maxAtb, currentAtb;

    public Category patronType;


    int EvaluateDamage( GameState state, Damage damage) {
        // apply modifiers
        foreach(var e in state.effectStack) {
            damage = e.ModifyDamage(damage);
        }

        return damage[patronType];
    }


   int EvaluateATB( GameState state) {
        
        var atb = 1;

        foreach(var e in state.effectStack) {
            atb = e.ModifyATB(atb);
        }

        return atb;
    }

}


public struct Damage{
    public int[] amounts = new int[(int)Category.Count];

    public Damage(){}

    public int this[Category c]
    {
        get { return amounts[(int)c]; }
        set { amounts[(int)c] = value; }
    }

}

public abstract class Punchline {
    
    public abstract Damage GetDamage();

}


public abstract class Setup  {
    
    public virtual Damage ModifyDamage(Damage d) {return d;}
    public virtual int ModifyATB(int atb) {return atb;}


}

class GameState {


    

    public List<Setup> effectStack = new List<Setup>();

    // punchline is a mix of positive or negative categories.
    // example setup: times two carnivore, minus one ground.
    // example punchline: one ground, -one carnivore
    // effect: deal negative two carnivore entertainment, zero ground entertainment.


}