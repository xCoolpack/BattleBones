using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : MonoBehaviour
{
    public static Player playerComponent;
    public EvaluationEngine evaluationEngine;

    void Awake()
    {
        playerComponent = GameObject.Find("Player2").GetComponent<Player>();
    }

    void Update()
    {
        if (playerComponent.IsPlayersTurn())
            ProcessTurn();
    }

    public void ProcessTurn()
    {
        
    }

    public void GenerateMoves()
    {
        
    }

    public void SelectMoves()
    {

    }
}
