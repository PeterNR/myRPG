using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] private Text _messageText;

    private PartyMemberUI[] _memberSlots;

    public void Initialize()
    {
        _memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Nuzlon> nuzlon)
    {
        for(int i=0; i < _memberSlots.Length; i++)
        {
            if(i<nuzlon.Count)
            {
                _memberSlots[i].SetHUD(nuzlon[i]);
            }
            else
            {
                _memberSlots[i].gameObject.SetActive(false); ;
            }
        }
        _messageText.text = "Choose a Nuzlon";
    }
}
