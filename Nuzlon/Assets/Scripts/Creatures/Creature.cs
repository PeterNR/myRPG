using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature 
{
    public BaseCreature Base { get; set; }
    public int Level { get; set; }

    public int CurrentHP { get; set; }
    public List<Move> Moves { get; set; }

    public Creature(BaseCreature cBase, int cLevel)
    {
        Base = cBase;
        Level = cLevel;
        CurrentHP = MaxHp;

        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if(move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));

                if((Moves.Count >= 4))
                {
                    break;
                }
            }
        }
    }


    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }
    public int MaxHp
    {
        get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10; }
    }
    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }
    public int SpecialAttack
    {
        get { return Mathf.FloorToInt((Base.SpecialAttack * Level) / 100f) + 5; }
    }
    public int SpecialDefense
    {
        get { return Mathf.FloorToInt((Base.SpecialDefense * Level) / 100f) + 5; }
    }

    public DamageDetails TakeDamage(Move move, Creature attacker)
    {
        //this is the damage magic. This is where you must look at the numbers to see how much damage moves should do
        float typeEffect = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        DamageDetails details = new DamageDetails()
        {
            Fainted = false,
            TypeEffect = typeEffect
        };

        float modifiers = Random.Range(0.85f, 1f) * typeEffect;
        float a = (2f * attacker.Level + 10f) / 50f;
        float d= a * move.Base.Power  * ((float)attacker.Attack / Defense) + 2f;
        int damage = Mathf.FloorToInt(d * modifiers);
        Debug.Log($"Damage is {damage} max hps is {MaxHp}");

        CurrentHP -= damage;
        if(CurrentHP<=0)
        {
            CurrentHP = 0;
            details.Fainted = true;
        }
        return details;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float TypeEffect { get; set; }
}
