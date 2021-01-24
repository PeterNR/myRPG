using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField]
    private Text _nameText, _levelText;
    [SerializeField]
    private HPBar hpBar;
    //public Slider HpSlider;

    Creature _creature;

    public void SetHUD(Creature creature)
    {
        _creature = creature;

        _nameText.text = creature.Base.Name;
        _levelText.text = "Lvl " + creature.Level;
        hpBar.SetHP((float)creature.CurrentHP / creature.MaxHp);
       // HpSlider.maxValue = unit.MaxHP;
        //HpSlider.value = unit.CurrentHP;
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_creature.CurrentHP / _creature.MaxHp);
        Debug.Log($"{_nameText.text} is at {_creature.CurrentHP}");
    }
}
