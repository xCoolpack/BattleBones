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

    public TextMeshProUGUI TurnText;

    // Start is called before the first frame update
    void Start()
    {
        _playerIndex = 0;
        CurrentPlayer = Players[_playerIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn(){
        Players[_playerIndex].PlayerTurnHandler.EndOfTurn();
        TurnCounter++;
        NextPlayer();
        Players[_playerIndex].PlayerTurnHandler.StartOfTurn();
        TurnText.text = "Turn: " + Players[_playerIndex].name;
    }

    private void NextPlayer()
    {
        if (_playerIndex < Players.Count - 1)
        {
            _playerIndex++;
        }
        else
        {
            _playerIndex = 0;
        }

        CurrentPlayer = Players[_playerIndex];
    }
}
