using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    public int TurnCounter;
    public GameObject CurrentPlayer { get; private set; }

    private int _playerIndex;
    public List<GameObject> Players;

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

    public void NextPlayer()
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
