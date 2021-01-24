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

    public bool TakeDamage(Move move, Creature attacker)
    {
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250;
        float d= a * move.Base.Power * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);
        Debug.Log($"Damage is {damage} max hps is {MaxHp}");

        CurrentHP -= damage;
        if(CurrentHP<=0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}
