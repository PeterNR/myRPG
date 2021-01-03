using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField]
    private BaseCreature _base;
    [SerializeField]
    private int _level;
    [SerializeField]
    private bool _isPlayerUnit;

    public Creature BattleCreature { get; set; }

    public void Setup()
    {
        BattleCreature = new Creature(_base, _level);
        if(_isPlayerUnit)
        {
            GetComponent<Image>().sprite = BattleCreature.Base.BackSprite;
        }
        else
        {
            GetComponent<Image>().sprite = BattleCreature.Base.FrontSprite;
        }
    }
}
