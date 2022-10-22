using System.Linq;
using UnityEngine;

public class Mission2SideObjective1 : MonoBehaviour, IObjective
{
    [SerializeField]
    private Player _player;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $"Current farms count: {_player.Buildings.Count(b => b.BaseBuildingStats.BuildingName == "Farm")}/6";

    private bool CompletionCheck()
    {
        return _player.Buildings.Count(b => b.BaseBuildingStats.BuildingName == "Farm") >= 6;
    }
}
