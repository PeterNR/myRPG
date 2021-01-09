using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    private BattleUnit _playerUnit, _enemyUnit;
    [SerializeField]
    private BattleHud _playerHUD, _enemyHUD;
    [SerializeField]
    BattleDialogueBox _dialogueBox;

    private int _currentActionIndex, _currentMoveIndex;
    private bool _usingVerticalAxis = false, _usingHorizontalAxis = false;


    private BattleState state;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        _playerUnit.Setup();
        _playerHUD.SetHUD(_playerUnit.BattleCreature);
        _enemyUnit.Setup();
        _enemyHUD.SetHUD(_playerUnit.BattleCreature);

        _dialogueBox.SetMoveNames(_playerUnit.BattleCreature.Moves);

         yield return _dialogueBox.TypeDialogue($"A wild {_enemyUnit.BattleCreature.Base.name} appeared.");

        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    private void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(_dialogueBox.TypeDialogue("Choose an action"));
        _dialogueBox.EnableActionSelector(true);
    }

    private void PlayerMove()
    {
        state = BattleState.PlayerMove;
        _dialogueBox.EnableActionSelector(false);
        _dialogueBox.EnableDialogueText(false);
        _dialogueBox.EnableMoveSelector(true);
    }

    private void Update()
    {
        if(state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    //weird way of choosing actions in battle
    private void HandleActionSelection()
    {
        if(Input.GetAxisRaw("Vertical")<0)
        {
            if(_currentActionIndex < 1)
            {
                _currentActionIndex++;
            }
        }else if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (_currentActionIndex > 0)
            {
                _currentActionIndex--;
            }
        }

        _dialogueBox.UpdateActionSelection(_currentActionIndex);

        if(Input.GetButtonDown("Jump"))
        {
            if(_currentActionIndex == 0)
            {
                //fight
                PlayerMove();
            }else if(_currentActionIndex == 1)
            {
                //run
            }
        }
    }

    private void HandleMoveSelection()
    {
        if (Input.GetAxisRaw("Horizontal") > 0 && !_usingHorizontalAxis)
        {
            _usingHorizontalAxis = true;
            if (_currentMoveIndex < _playerUnit.BattleCreature.Moves.Count -1)
            {
                _currentMoveIndex++;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && !_usingHorizontalAxis)
        {
            _usingHorizontalAxis = true;
            if (_currentMoveIndex > 0)
            {
                _currentMoveIndex--;
            }
        }else if (Input.GetAxisRaw("Vertical") < 0 && !_usingVerticalAxis)
        {
            _usingVerticalAxis = true;
            if (_currentMoveIndex < _playerUnit.BattleCreature.Moves.Count - 2)
            {
                _currentMoveIndex += 2;
            }
        }
        else if (Input.GetAxisRaw("Vertical") > 0 && !_usingVerticalAxis)
        {
            _usingVerticalAxis = true;
            if (_currentMoveIndex > 1)
            {
                _currentMoveIndex -= 2;
            }
        }else if(Input.GetAxisRaw("Horizontal") == 0)
        {
            _usingHorizontalAxis = false;
        }else if(Input.GetAxisRaw("Vertical") == 0)
        {
            _usingVerticalAxis = false;
        }


        _dialogueBox.UpdateMoveSelection(_currentMoveIndex, _playerUnit.BattleCreature.Moves[_currentMoveIndex]);
    }
}
