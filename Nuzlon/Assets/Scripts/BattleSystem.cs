using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Won, Lost }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject PlayerPrefab, EnemyPrefab;

    public Transform PlayerBattleStation, EnemyBattleStation;

    public Text Dialoguetext;

    public BattleHud PlayerHUD, EnemyHUD;

    [SerializeField]
    private float _textDelayTime;

    private Unit _playerUnit;
    private Unit _enemyUnit;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {

        GameObject playerGO = Instantiate(PlayerPrefab, PlayerBattleStation);
        _playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(EnemyPrefab, EnemyBattleStation);
        _enemyUnit = enemyGO.GetComponent<Unit>();

        Dialoguetext.text = "A wild " + _enemyUnit.UnitName + " approaches";

        PlayerHUD.SetHUD(_playerUnit);
        EnemyHUD.SetHUD(_enemyUnit);

        yield return new WaitForSeconds(_textDelayTime);

        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

   

    private void EndBattle()
    {
        if(state == BattleState.Won)
        {
            Dialoguetext.text = "You won the battle!";
        }else if( state == BattleState.Lost)
        {
            Dialoguetext.text = "You were defeated.";
        }
    }

    private IEnumerator EnemyTurn()
    {
        Dialoguetext.text = _enemyUnit.UnitName + " attacks";

        yield return new WaitForSeconds(_textDelayTime);

        bool isDead = _playerUnit.TakeDamage(_enemyUnit.Damage);

        PlayerHUD.SetHP(_playerUnit.CurrentHP);

        yield return new WaitForSeconds(_textDelayTime);

        if(isDead)
        {
            state = BattleState.Lost;
            EndBattle();
        }
        else
        {
            state = BattleState.PlayerTurn;
            PlayerTurn();
        }
    }

    private IEnumerator PlayerAttack()
    {
        //deal dmg
        bool isDead = _enemyUnit.TakeDamage(_playerUnit.Damage);

        EnemyHUD.SetHP(_enemyUnit.CurrentHP);
        Dialoguetext.text = "The attack is succesfull";

        yield return new WaitForSeconds(_textDelayTime);

        if (isDead)
        {
            state = BattleState.Won;
            EndBattle();
        }
        else
        {
            state = BattleState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator PlayerHeal()
    {
        //deal dmg
        _playerUnit.Heal(5);

        PlayerHUD.SetHP(_playerUnit.CurrentHP);
        Dialoguetext.text = "You healed";

        yield return new WaitForSeconds(_textDelayTime);

        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    private void PlayerTurn()
    {
        Dialoguetext.text = "Choose an action";
    }

    public void OnAttackBtn()
    {
        if(state != BattleState.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }

    public void OnHealBtn()
    {
        if(state != BattleState.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerHeal());
    }


}
