using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature 
{
    private BaseCreature _base;
    int _level;

    public int CurrentHP { get; set; }
    public List<Move> Moves { get; set; }

    public Creature(BaseCreature cBase, int cLevel)
    {
        _base = cBase;
        _level = cLevel;
        CurrentHP = MaxHp;

        Moves = new List<Move>();
        foreach (var move in _base.LearnableMoves)
        {
            if(move.Level <= _level)
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
        get { return Mathf.FloorToInt((_base.Speed * _level) / 100f) + 5; }
    }
    public int MaxHp
    {
        get { return Mathf.FloorToInt((_base.MaxHP * _level) / 100f) + 10; }
    }
    public int Attack
    {
        get { return Mathf.FloorToInt((_base.Attack * _level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((_base.Defense * _level) / 100f) + 5; }
    }
    public int SpecialAttack
    {
        get { return Mathf.FloorToInt((_base.SpecialAttack * _level) / 100f) + 5; }
    }
    public int SpecialDefense
    {
        get { return Mathf.FloorToInt((_base.SpecialDefense * _level) / 100f) + 5; }
    }
}
