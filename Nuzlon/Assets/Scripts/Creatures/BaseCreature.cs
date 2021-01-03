using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Type
{
    fire,
    water,
    ground,
    wind,
    tech,
    acid
}

[CreateAssetMenu(fileName = "Nuzlon", menuName = "Nuzlon/Create new creature")]

public class BaseCreature : ScriptableObject
{
    [SerializeField]
    private string name;

    [TextArea]
    [SerializeField]
    private string description;
    [SerializeField]
    private Sprite backsprite, frontsprite;

    [SerializeField]
    private int speed, maxHp, attack, defense, specialAttack, specialDefense;

    [SerializeField]
    Type type1, type2;
    [SerializeField]
    private List<LearnableMove> learnableMoves = new List<LearnableMove>();



    public string Name
    {
        get { return name; }
    }
    public string Description
    {
        get { return description; }
    }
    public Sprite BackSprite
    {
        get { return backsprite; }
    }
    public Sprite FrontSprite
    {
        get { return frontsprite; }
    }
    public int Speed
    {
        get { return speed; }
    }
    public int MaxHP
    {
        get { return maxHp; }
    }
    public int Attack
    {
        get { return attack; }
    }
    public int Defense
    {
        get { return defense; }
    }
    public int SpecialAttack
    {
        get { return specialAttack; }
    }
    public int SpecialDefense
    {
        get { return specialDefense; }
    }
    public Type Type1
    {
        get { return type1; }
    }
    public Type Type2
    {
        get { return type2; }
    }
    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField]
    private BaseMove baseMove;
    [SerializeField]
    private int level;

    public BaseMove Base
    {
        get { return baseMove; }
    }
    public int Level
    {
        get { return level; }
    }
}
