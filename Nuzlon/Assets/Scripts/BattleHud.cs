using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    public Text NameText, LevelText;
    public Slider HpSlider;

    public void SetHUD(Unit unit)
    {
        NameText.text = unit.UnitName;
        LevelText.text = "Lvl " + +unit.UnitLevel;
        HpSlider.maxValue = unit.MaxHP;
        HpSlider.value = unit.CurrentHP;
    }

    public void SetHP(int hp)
    {
        HpSlider.value = hp;
    }
}
