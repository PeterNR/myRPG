using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] private Text _messageText;

    private PartyMemberUI[] _memberSlots;
    List<Nuzlon> _nuzlonList;

    public void Initialize()
    {
        _memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Nuzlon> nuzlonList)
    {
        _nuzlonList = nuzlonList;

        for(int i=0; i < _memberSlots.Length; i++)
        {
            if(i<nuzlonList.Count)
            {
                _memberSlots[i].SetPartyHUD(nuzlonList[i]);
            }
            else
            {
                _memberSlots[i].gameObject.SetActive(false); ;
            }
        }
        _messageText.text = "Choose a Nuzlon";
    }


    public void UpdateMemberSelection(int selectedMember)
    {
        for(int i = 0; i<_nuzlonList.Count; i++)
        {
            if (i==selectedMember)
            {
                _memberSlots[i].SetSelected(true);
            }
            else
            {
                _memberSlots[i].SetSelected(false);
            }
        }
    }

    public void SetMessageText(string message)
    {
        _messageText.text = message;
    }
}
