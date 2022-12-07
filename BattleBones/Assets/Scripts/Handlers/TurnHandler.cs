using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Logger.Log($"Turn: {TurnCounter}");
        Logger.Log("Human turn");
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
    }

    private void NextPlayer()
    {
        if (_humanTurn)
        {
            CurrentPlayer = ComputerPlayerObj.playerComponent;
            Logger.Log("Bot turn");
        }
        else
        {
            CurrentPlayer = HumanPlayer;
            TurnCounter++;
            GlobalEventHandler.TurnEnd();
            Logger.Log($"Turn: {TurnCounter}");
            Logger.Log("Human turn");
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
        Logger.Log("Game won");
        ObjectiveHandler.Objectives.ForEach(objective => 
            PlayerPrefs.SetInt(objective.ObjectiveId.ToString(), objective.IsCompleted ? 1 : 0));
        var name = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt(name.Substring(0, name.Length - 5), 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MissionWinScene");
    }

    /// <summary>
    /// Method ending the mission when player lost
    /// </summary>
    private void GameOver()
    {
        Logger.Log("Game over");
        SceneManager.LoadScene("MissionLoseScene");
    }

    public bool IsPlayersTurn(Player player)
    {
        return CurrentPlayer == player;
    }
}
