using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle}

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private BattleSystem _battlesystem;
    [SerializeField]
    private Camera _worldCamera;

    private GameState state;

    private void Start()
    {
        _playerController.OnEncounter += StartBattle;
        _battlesystem.OnBattleOver += EndBattle;
    }

    private void Update()
    {
        if(state == GameState.FreeRoam)
        {
            _playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            _battlesystem.HandleUpdate();
        }
    }

    private void StartBattle()
    {
        state = GameState.Battle;
        _battlesystem.gameObject.SetActive(true);
        _worldCamera.gameObject.SetActive(false);

        NuzlonParty playerParty = _playerController.GetComponent<NuzlonParty>();
        Nuzlon wildNuzlon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildNuzlon();

        _battlesystem.StartBattle(playerParty, wildNuzlon);
    }

    private void EndBattle(bool playerWon)
    {
        state = GameState.FreeRoam;
        _battlesystem.gameObject.SetActive(false);
        _worldCamera.gameObject.SetActive(true);
    }
}
