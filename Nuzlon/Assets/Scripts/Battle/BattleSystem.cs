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

    private BattleState state;

    private bool _pressedBtn = false;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        _playerUnit.Setup();
        _playerHUD.SetHUD(_playerUnit.BattleCreature);
        _enemyUnit.Setup();
        _enemyHUD.SetHUD(_enemyUnit.BattleCreature);

        _dialogueBox.SetMoveNames(_playerUnit.BattleCreature.Moves);

         yield return _dialogueBox.TypeDialogue($"A wild {_enemyUnit.BattleCreature.Base.name} appeared.");

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

    private IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;
        Move move = _playerUnit.BattleCreature.Moves[_currentMoveIndex];
        yield return _dialogueBox.TypeDialogue($"{_playerUnit.BattleCreature.Base.Name } used {move.Base.Name}");

        _playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        _enemyUnit.PlayHitAnimation();

        DamageDetails damageDetails = _enemyUnit.BattleCreature.TakeDamage(move, _playerUnit.BattleCreature);
        yield return _enemyHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return _dialogueBox.TypeDialogue($"{_enemyUnit.BattleCreature.Base.Name} fainted");
            _enemyUnit.PlayFaintAnimation();
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    private IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        Move move = _enemyUnit.BattleCreature.GetRandomMove();

        yield return _dialogueBox.TypeDialogue($"{_enemyUnit.BattleCreature.Base.Name } used {move.Base.Name}");

        _enemyUnit.PlayAttackAnimation();
        new WaitForSeconds(1f);

        _playerUnit.PlayHitAnimation();

        DamageDetails damageDetails = _playerUnit.BattleCreature.TakeDamage(move, _enemyUnit.BattleCreature);
        yield return _playerHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return _dialogueBox.TypeDialogue($"{_playerUnit.BattleCreature.Base.Name} fainted");
            _playerUnit.PlayFaintAnimation();
        }
        else
        {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if(damageDetails.TypeEffect>1)
        {
            yield return _dialogueBox.TypeDialogue("It's extra damaging");
        }else if(damageDetails.TypeEffect == 0 )
        {
            yield return _dialogueBox.TypeDialogue("It's not damaging at all");
        }else if(damageDetails.TypeEffect < 1)
        {
            yield return _dialogueBox.TypeDialogue("It's not very damaging");
        }
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
        if(!_pressedBtn)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (_currentMoveIndex < _playerUnit.BattleCreature.Moves.Count - 1)
                {
                    _currentMoveIndex++;
                    _pressedBtn = true;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (_currentMoveIndex > 0)
                {
                    _currentMoveIndex--;
                    _pressedBtn = true;
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (_currentMoveIndex < _playerUnit.BattleCreature.Moves.Count - 2)
                {
                    _currentMoveIndex += 2;
                    _pressedBtn = true;
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (_currentMoveIndex > 1)
                {
                    _currentMoveIndex -= 2;
                    _pressedBtn = true;
                }
            }
            _dialogueBox.UpdateMoveSelection(_currentMoveIndex, _playerUnit.BattleCreature.Moves[_currentMoveIndex]);
        }

        if(Input.GetAxisRaw("Vertical") ==0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            _pressedBtn = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            _dialogueBox.EnableMoveSelector(false);
            _dialogueBox.EnableDialogueText(true);
            StartCoroutine(PerformPlayerMove());
        }

    }
}
