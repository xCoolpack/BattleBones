using System.Linq;
using UnityEngine;

public class Mission4SideObjective2 : MonoBehaviour, IObjective
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
