using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission3SideObjective1 : MonoBehaviour, IObjective
{
    [SerializeField] private TurnHandler _turnHandler;
    [SerializeField] private int _turnCounter;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $"Current turn: {_turnHandler.TurnCounter}/{_turnCounter}";

    private bool CompletionCheck()
    {
        return _turnHandler.TurnCounter <= _turnCounter;
    }
}
