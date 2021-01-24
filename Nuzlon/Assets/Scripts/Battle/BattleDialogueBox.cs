using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogueBox : MonoBehaviour
{
    [SerializeField]
    private Text _dialogueText;
    [SerializeField]
    private int _letterPerSecond;
    [SerializeField]
    private GameObject _actionSelector, _moveSelector, _moveDetails;
    [SerializeField]
    private List<Text> _actionText, _moveText;
    [SerializeField]
    private Text ppText, _moveTypeText;
    [SerializeField]
    private Color _highlightedColor, _regularColor;


    public void SetDialogue(string dialogue)
    {
        _dialogueText.text = dialogue;
    }

    public IEnumerator TypeDialogue(string dialogue)
    {
        _dialogueText.text = "";
        foreach(char letter in dialogue.ToCharArray()){
            _dialogueText.text += letter;
            yield return new WaitForSeconds(1/_letterPerSecond);
        }
    }

    public void EnableDialogueText(bool enabled)
    {
        _dialogueText.enabled = enabled;
    }
    public void EnableMoveSelector(bool enabled)
    {
        _moveSelector.SetActive(enabled);
        _moveDetails.SetActive(enabled);
    }
    public void EnableActionSelector(bool enabled)
    {
        _actionSelector.SetActive(enabled);
    }
    public void UpdateActionSelection(int selectedAction)
    {
        for(int i=0; i<_actionText.Count; i++)
        {
            if(i == selectedAction)
            {
                _actionText[i].color = _highlightedColor;
            }
            else
            {
                _actionText[i].color = _regularColor;
            }
        }
    }
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for(int i=0; i<_moveText.Count; i++)
        {
            if(i == selectedMove)
            {
                _moveText[i].color = _highlightedColor;
            }
            else
            {
                _moveText[i].color = _regularColor;
            }

            ppText.text = $"PP {move.PP}/{move.Base.PP}";
            _moveTypeText.text = move.Base.Type.ToString();
        }
    }

    public void SetMoveNames(List<Move> moves)
    {
        for(int i=0; i<_moveText.Count;i++)
        {
            if(i<moves.Count)
            {
                _moveText[i].text = moves[i].Base.Name;
            }
            else
            {
                _moveText[i].text = "-";
            }
        }
    }
}
