using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogueBox : MonoBehaviour
{
    [SerializeField]
    private Text _dialogueText;
    [SerializeField]
    private int _letterrPerSecond;
    [SerializeField]
    private GameObject _actionSelector, _moveSelector, _moveDetails;
    [SerializeField]
    private List<Text> _actionText, _moveText;
    [SerializeField]
    private Text ppText, _moveTypeText;


    public void SetDialogue(string dialogue)
    {
        _dialogueText.text = dialogue;
    }

    public IEnumerator TypeDialogue(string dialogue)
    {
        _dialogueText.text = "";
        foreach(char letter in dialogue.ToCharArray()){
            _dialogueText.text += letter;
            yield return new WaitForSeconds(1/_letterrPerSecond);
        }
    }

    public void EnableDialogueText(bool enabled)
    {
        _dialogueText.enabled = enabled;
    }
    public void EnableMoveSelector(bool enabled)
    {
        _actionSelector.SetActive(enabled);
        _moveDetails.SetActive(enabled);
    }
    public void EnableActionSelector(bool enabled)
    {
        _actionSelector.SetActive(enabled);
    }
}
