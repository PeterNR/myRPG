using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Won, Lost }

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    private BattleUnit _playerUnit, _enemyUnit;
    [SerializeField]
    private BattleHud _playerHUD, _enemyHUD;

    private void Start()
    {
        SetupBattle();
    }

    public void SetupBattle()
    {
        _playerUnit.Setup();
        _playerHUD.SetHUD(_playerUnit.BattleCreature);
        _enemyUnit.Setup();
        _enemyHUD.SetHUD(_playerUnit.BattleCreature);
    }
}
