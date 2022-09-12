using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnHandler : MonoBehaviour
{
    public int TurnCounter;
    public Player CurrentPlayer { get; private set; }
    private int _playerIndex;
    public List<Player> Players;
    public TextMeshProUGUI PlayerTurnText;
    public TextMeshProUGUI TurnCountText;
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
        PlayerTurnText.text = "Turn: " + Players[_playerIndex].name;
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
            TurnCountText.text = $"Turn {TurnCounter}";
        }

        CurrentPlayer = Players[_playerIndex];
    }
}
