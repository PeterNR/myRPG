using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Nuzlon", menuName = "Nuzlon/Create new nuzlon")]

public class BaseNuzlon : ScriptableObject
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
    NuzlonType type1, type2;
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
    public NuzlonType Type1
    {
        get { return type1; }
    }
    public NuzlonType Type2
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

public enum NuzlonType
{
    none,
    Fire,
    Water,
    Earth,
    Air,
    Tech,
    Acid
}

public class TypeChart
{
    static float[][] chart =
    {
        //                    fire/water/earth/air  /tech/acid 
       /*fire*/ new float[]   {0.5f, 0f, 0.5f, 1f,   2f,   2f},
       /*water*/ new float[]  {2f,   1f, 0.5f, 1f,   2f,   0.5f},
       /*earth*/ new float[]  {1f,   0f, 1f,   0.5f, 0.5f, 1f},
       /*air*/ new float[]    {0.5f, 2f, 0.5f, 0.5f, 0f,   0,5f},
       /*tech*/ new float[]   {0.5f, 1f, 2f,   2f,   2f,   0f},
       /*acid*/ new float[]   {0f,   2f, 2f,   0f,   2f,   0.5f}

    };

    public static float GetEffectiveness (NuzlonType attackType, NuzlonType defenseType)
    {
        if(attackType == NuzlonType.none || defenseType == NuzlonType.none)
        {
            return 1f;
        }

        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}