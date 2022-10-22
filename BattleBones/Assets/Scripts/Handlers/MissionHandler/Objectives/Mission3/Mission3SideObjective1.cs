using System.Linq;
using UnityEngine;

public class Mission3SideObjective1 : MonoBehaviour, IObjective
{
    [SerializeField]
    private TurnHandler _turnHandler;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $"Current turn: {_turnHandler.TurnCounter}/30";

    private bool CompletionCheck()
    {
        return _turnHandler.TurnCounter <= 30;
    }
}
