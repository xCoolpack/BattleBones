using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    public int TurnCounter;
    public Player CurrentPlayer { get; private set; }
    private int _playerIndex;
    public List<Player> Players;
    public EventHandler GlobalEventHandler;

    // Start is called before the first frame update
    void Start()
    {
        _playerIndex = 0;
        CurrentPlayer = Players[_playerIndex];
    }

    public void NextTurn(){
        Players[_playerIndex].PlayerEventHandler.TurnEnd();
        NextPlayer();
        Players[_playerIndex].PlayerEventHandler.TurnStart();
        Players[_playerIndex].ResourceManager.GenerateIncome();
        Players[_playerIndex].RestoreUnitsMovementPoints();
        Players[_playerIndex].ApplyUnitsModifiers();
    }

    private void NextPlayer()
    {
        if (_playerIndex < Players.Count - 1)
        {
            _playerIndex++;
        }
        else
        {
            // End of global turn
            _playerIndex = 0;
            TurnCounter++;
            GlobalEventHandler.TurnEnd();
            GlobalEventHandler.TurnStart();
        }

        CurrentPlayer = Players[_playerIndex];
    }

    public bool IsPlayersTurn(Player player)
    {
        return CurrentPlayer == player;
    }
}
