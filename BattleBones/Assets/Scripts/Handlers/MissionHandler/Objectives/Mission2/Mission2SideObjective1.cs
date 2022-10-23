using System.Linq;
using UnityEngine;

public class Mission2SideObjective1 : MonoBehaviour, IObjective
{
    [SerializeField] private Player _player;
    [SerializeField] private int _farmCount;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $"Current farms count: {_player.Buildings.Count(b => b.BaseBuildingStats.BuildingName == "Farm")}/{_farmCount}";

    private bool CompletionCheck()
    {
        return _player.Buildings.Count(b => b.BaseBuildingStats.BuildingName == "Farm") >= _farmCount;
    }
}
