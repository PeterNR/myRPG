﻿using System.Collections;
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

    Nuzlon _nuzlon;

    public void SetHUD(Nuzlon nuzlon)
    {
        _nuzlon = nuzlon;

        _nameText.text = nuzlon.Base.Name;
        _levelText.text = "Lvl " + nuzlon.Level;
        hpBar.SetHP((float)nuzlon.CurrentHP / nuzlon.MaxHp);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_nuzlon.CurrentHP / _nuzlon.MaxHp);
    }
}
