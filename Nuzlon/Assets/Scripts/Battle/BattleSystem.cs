using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, Busy, PartyScreen, BattleOver }

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    private BattleUnit _playerUnit, _enemyUnit;
    [SerializeField]
    BattleDialogueBox _dialogueBox;
    [SerializeField]
    private PartyScreen _partyScreen;

    public event Action<bool> OnBattleOver;

    NuzlonParty _playerParty;
    Nuzlon _wildNuzlon;

    private int _currentActionIndex, _currentMoveIndex, _currentPartyMemberIndex;
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
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    public IEnumerator SetupBattle()
    {
        _playerUnit.Setup(_playerParty.GetHealthyNuzlon());
        _enemyUnit.Setup(_wildNuzlon);

        _partyScreen.Initialize();

        _dialogueBox.SetMoveNames(_playerUnit.BattleNuzlon.Moves);

         yield return _dialogueBox.TypeDialogue($"A wild {_enemyUnit.BattleNuzlon.Base.name} appeared.");

        ActionSelection();
    }

    private void BattleOver(bool victory)
    {
        state = BattleState.BattleOver;
        OnBattleOver(victory);
    }

    private void ActionSelection()
    {
        state = BattleState.ActionSelection;
       _dialogueBox.SetDialogue(("Choose an action"));
        _dialogueBox.EnableActionSelector(true);
    }

    private void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        _partyScreen.SetPartyData(_playerParty.NuzlonList);
        _partyScreen.gameObject.SetActive(true);
    }

    private void MoveSelection()
    {
        state = BattleState.MoveSelection;
        _dialogueBox.EnableActionSelector(false);
        _dialogueBox.EnableDialogueText(false);
        _dialogueBox.EnableMoveSelector(true);
    }

    private IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;

        Move move = _playerUnit.BattleNuzlon.Moves[_currentMoveIndex];
        yield return RunMove(_playerUnit, _enemyUnit, move);

        if(state == BattleState.PerformMove)
        {
            StartCoroutine(EnemyMove());
        }
    }

    private IEnumerator EnemyMove()
    {
        state = BattleState.PerformMove;

        Move move = _enemyUnit.BattleNuzlon.GetRandomMove();
        yield return RunMove(_enemyUnit, _playerUnit, move);

        if (state == BattleState.PerformMove)
        {
            ActionSelection();
        }
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        move.PP--;
        yield return _dialogueBox.TypeDialogue($"{sourceUnit.BattleNuzlon.Base.Name } used {move.Base.Name}");

        sourceUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        targetUnit.PlayHitAnimation();

        DamageDetails damageDetails = targetUnit.BattleNuzlon.TakeDamage(move, sourceUnit.BattleNuzlon);
        yield return targetUnit.Hud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return _dialogueBox.TypeDialogue($"{targetUnit.BattleNuzlon.Base.Name} fainted");
            targetUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);

            CheckForBattleOver(targetUnit);
        }
    }

    private void CheckForBattleOver (BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            Nuzlon nextNuzlon = _playerParty.GetHealthyNuzlon();
            if (nextNuzlon != null)
            {
                OpenPartyScreen();
            }
            else
            {
                BattleOver(false);
            }
        }
        else
        {
            BattleOver(true);
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
                MoveSelection();
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
            StartCoroutine(PlayerMove());
        }else if(Input.GetButtonDown("Fire1"))
        {
            _dialogueBox.EnableMoveSelector(false);
            _dialogueBox.EnableDialogueText(true);
            ActionSelection();
        }

    }

    private void HandlePartySelection()
    {
        if (!_pressedBtn)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (_currentPartyMemberIndex < _playerParty.NuzlonList.Count - 1)
                {
                    _currentPartyMemberIndex++;
                    _pressedBtn = true;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (_currentPartyMemberIndex > 0)
                {
                    _currentPartyMemberIndex--;
                    _pressedBtn = true;
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (_currentPartyMemberIndex < _playerParty.NuzlonList.Count - 2)
                {
                    _currentPartyMemberIndex += 2;
                    _pressedBtn = true;
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (_currentPartyMemberIndex > 1)
                {
                    _currentPartyMemberIndex -= 2;
                    _pressedBtn = true;
                }
            }

            _partyScreen.UpdateMemberSelection(_currentPartyMemberIndex);
        }

        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            _pressedBtn = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Nuzlon selectedMember = _playerParty.NuzlonList[_currentPartyMemberIndex];
            if(selectedMember.CurrentHP <=0)
            {
                _partyScreen.SetMessageText("You can't send out a fainted Nuzlon");
                return;
            }
            if(selectedMember == _playerUnit.BattleNuzlon)
            {
                _partyScreen.SetMessageText("That Nuzlon is already out");
                return;
            }

            //_dialogueBox.EnableMoveSelector(false);
            //_dialogueBox.EnableDialogueText(true);
            _partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchNuzlon(selectedMember));
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            if (_playerUnit.BattleNuzlon.CurrentHP <= 0)
            {
                return;
            }
            _partyScreen.gameObject.SetActive(false);
            ActionSelection();
        }
    }

    IEnumerator SwitchNuzlon(Nuzlon newNuzlon)
    {
        if(_playerUnit.BattleNuzlon.CurrentHP>0)
        {
            yield return _dialogueBox.TypeDialogue($"Comeback {_playerUnit.BattleNuzlon.Base.name}!");
            _playerUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        _playerUnit.Setup(newNuzlon);

        _dialogueBox.SetMoveNames(newNuzlon.Moves);

        yield return _dialogueBox.TypeDialogue($"Go {newNuzlon.Base.name}!");

        StartCoroutine(EnemyMove());
    }
}
