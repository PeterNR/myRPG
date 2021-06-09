using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField]
    private Text _nameText, _levelText;
    [SerializeField]
    private HPBar _hpBar;

    [SerializeField]
    private Color _highlightedColor, _regularColor;


    Nuzlon _nuzlon;

    public void SetPartyHUD(Nuzlon nuzlon)
    {
        _nuzlon = nuzlon;

        _nameText.text = nuzlon.Base.Name;
        _levelText.text = "Lvl " + nuzlon.Level;
        _hpBar.SetHP((float)nuzlon.CurrentHP / nuzlon.MaxHp);
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            _nameText.color = _highlightedColor;
        }
        else
        {
            _nameText.color = _regularColor;
        }
    }
}
