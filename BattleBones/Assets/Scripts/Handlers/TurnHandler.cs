using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnHandler : MonoBehaviour
{
    public int TurnCounter;
    public Player CurrentPlayer { get; private set; }
    private bool _humanTurn;
    public Player HumanPlayer;
    public ComputerPlayer ComputerPlayerObj;
    public EventHandler GlobalEventHandler;
    public ObjectiveHandler ObjectiveHandler;

    // Start is called before the first frame update
    void Start()
    {
        _humanTurn = true;
        CurrentPlayer = HumanPlayer;
    }

    public void NextTurn(){
        CurrentPlayer.PlayerEventHandler.TurnEnd();
        CheckMissionObjectives();
        NextPlayer();
        CurrentPlayer.PlayerEventHandler.TurnStart();
        CurrentPlayer.ResourceManager.GenerateIncome();
        CurrentPlayer.RestoreUnitsMovementPoints();
        CurrentPlayer.ApplyUnitsModifiers();
        if (!_humanTurn)
        {
            ComputerPlayerObj.ProcessTurn();
        }

         /*Players[_playerIndex].PlayerEventHandler.TurnEnd();
         NextPlayer();
         Players[_playerIndex].PlayerEventHandler.TurnStart();
         Players[_playerIndex].ResourceManager.GenerateIncome();
         Players[_playerIndex].RestoreUnitsMovementPoints();
         Players[_playerIndex].ApplyUnitsModifiers();*/

         
    }

    private void NextPlayer()
    {
        if (_humanTurn)
        {
            CurrentPlayer = ComputerPlayerObj.playerComponent;
        }
        else
        {
            CurrentPlayer = HumanPlayer;
            TurnCounter++;
            GlobalEventHandler.TurnEnd();
            GlobalEventHandler.TurnStart();
        }

        _humanTurn = !_humanTurn;
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
        var name = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt(name.Substring(0, name.Length - 5), 1);
        SceneManager.LoadScene("CampaignMapScene");
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
