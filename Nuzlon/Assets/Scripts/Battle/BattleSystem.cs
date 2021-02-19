using System;
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
    [SerializeField]
    private PartyScreen _partyScreen;

    public event Action<bool> OnBattleOver;

    NuzlonParty _playerParty;
    Nuzlon _wildNuzlon;

    private int _currentActionIndex, _currentMoveIndex;
    private BattleState state;
    private bool _pressedBtn = false;

    public void StartBattle(NuzlonParty playerParty, Nuzlon wildNuzlon)
    {
        _playerParty = playerParty;
        _wildNuzlon = wildNuzlon;
        StartCoroutine(SetupBattle());
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    public IEnumerator SetupBattle()
    {
        _playerUnit.Setup(_playerParty.GetHealthyNuzlon());
        _playerHUD.SetHUD(_playerUnit.BattleNuzlon);
        _enemyUnit.Setup(_wildNuzlon);
        _enemyHUD.SetHUD(_enemyUnit.BattleNuzlon);

        _partyScreen.Initialize();

        _dialogueBox.SetMoveNames(_playerUnit.BattleNuzlon.Moves);

         yield return _dialogueBox.TypeDialogue($"A wild {_enemyUnit.BattleNuzlon.Base.name} appeared.");

        PlayerAction();
    }

    private void PlayerAction()
    {
        state = BattleState.PlayerAction;
       _dialogueBox.SetDialogue(("Choose an action"));
        _dialogueBox.EnableActionSelector(true);
    }

    private void OpenPartyScreen()
    {
        _partyScreen.SetPartyData(_playerParty.NuzlonList);
        _partyScreen.gameObject.SetActive(true);
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
        Move move = _playerUnit.BattleNuzlon.Moves[_currentMoveIndex];
        move.PP--;
        yield return _dialogueBox.TypeDialogue($"{_playerUnit.BattleNuzlon.Base.Name } used {move.Base.Name}");

        _playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        _enemyUnit.PlayHitAnimation();

        DamageDetails damageDetails = _enemyUnit.BattleNuzlon.TakeDamage(move, _playerUnit.BattleNuzlon);
        yield return _enemyHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return _dialogueBox.TypeDialogue($"{_enemyUnit.BattleNuzlon.Base.Name} fainted");
            _enemyUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    private IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        Move move = _enemyUnit.BattleNuzlon.GetRandomMove();
        move.PP--;

        yield return _dialogueBox.TypeDialogue($"{_enemyUnit.BattleNuzlon.Base.Name } used {move.Base.Name}");

        _enemyUnit.PlayAttackAnimation();
        new WaitForSeconds(1f);

        _playerUnit.PlayHitAnimation();

        DamageDetails damageDetails = _playerUnit.BattleNuzlon.TakeDamage(move, _enemyUnit.BattleNuzlon);
        yield return _playerHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return _dialogueBox.TypeDialogue($"{_playerUnit.BattleNuzlon.Base.Name} fainted");
            _playerUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);

            Nuzlon nextNuzlon = _playerParty.GetHealthyNuzlon();
            if(nextNuzlon!=null)
            {
                _playerUnit.Setup(_playerParty.GetHealthyNuzlon());
                _playerHUD.SetHUD(_playerUnit.BattleNuzlon);

                _dialogueBox.SetMoveNames(_playerUnit.BattleNuzlon.Moves);

                yield return _dialogueBox.TypeDialogue($"Go {_playerUnit.BattleNuzlon.Base.name}!");

                PlayerAction();
            }
            else
            {
                OnBattleOver(false);
            }
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



    //weird way of choosing actions in battle
    private void HandleActionSelection()
    {
        if (!_pressedBtn)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                //if (_currentActionIndex < 3)
                {
                    _currentActionIndex++;
                    _pressedBtn = true;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                //if (_currentActionIndex > 0)
                {
                    _currentActionIndex--;
                    _pressedBtn = true;
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                //if (_currentActionIndex < 2)
                {
                    _currentActionIndex += 2;
                    _pressedBtn = true;
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                //if (_currentActionIndex > 1)
                {
                    _currentActionIndex -= 2;
                    _pressedBtn = true;
                }
            }
            _currentActionIndex = Mathf.Clamp(_currentActionIndex, 0, 3);
        }

            _dialogueBox.UpdateActionSelection(_currentActionIndex);

        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            _pressedBtn = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(_currentActionIndex == 0)
            {
                //fight
                PlayerMove();
            }else if(_currentActionIndex == 1)
            {
                //bag
            }else if(_currentActionIndex == 2)
            {
                //party
                OpenPartyScreen();
            }else if(_currentActionIndex == 3)
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
                if (_currentMoveIndex < _playerUnit.BattleNuzlon.Moves.Count - 1)
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
                if (_currentMoveIndex < _playerUnit.BattleNuzlon.Moves.Count - 2)
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
            _dialogueBox.UpdateMoveSelection(_currentMoveIndex, _playerUnit.BattleNuzlon.Moves[_currentMoveIndex]);
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
        }else if(Input.GetButtonDown("Fire1"))
        {
            _dialogueBox.EnableMoveSelector(false);
            _dialogueBox.EnableDialogueText(true);
            PlayerAction();
        }

    }
}
