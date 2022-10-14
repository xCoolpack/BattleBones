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
    public ObjectiveHandler ObjectiveHandler;

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

        //Debug.Log(ObjectiveHandler.CheckPrimaryObjectives());
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

    private void CheckMissionObjectives()
    {
        if (ObjectiveHandler.CheckFailObjectives())
        {
            GameOver();
        }
        else if (ObjectiveHandler.CheckPrimaryObjectives())
        {
            GameWon();
        }
    }

    /// <summary>
    /// Method ending the mission when player won
    /// </summary>
    private void GameWon()
    {
        Debug.Log("Game won");
        ObjectiveHandler.Objectives.ForEach(objective => 
            PlayerPrefs.SetInt(objective.ObjectiveId.ToString(), objective.IsCompleted ? 1 : 0));
    }

    /// <summary>
    /// Method ending the mission when player lost
    /// </summary>
    private void GameOver()
    {
        Debug.Log("Game over");
    }

    public bool IsPlayersTurn(Player player)
    {
        return CurrentPlayer == player;
    }
}
