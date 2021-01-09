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

    private int _currentActionIndex;

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

    private void Update()
    {
        if(state == BattleState.PlayerAction)
        {
            HandleActionSelection();    
        }
    }

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
            if (_currentActionIndex > 1)
            {
                _currentActionIndex--;
            }
        }
    }
}
