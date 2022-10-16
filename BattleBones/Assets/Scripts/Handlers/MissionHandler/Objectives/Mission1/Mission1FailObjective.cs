using System.Linq;
using UnityEngine;

public class Mission1FailObjective : MonoBehaviour, IObjective
{
    [SerializeField]
    private Player _player;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    [field: SerializeField]
    public string ObjectiveInfo { get; set; }

    private bool CompletionCheck()
    {
        var building =
            _player.Buildings.FirstOrDefault(building => building.gameObject.GetComponent<Outpost>() != null);
        return building == null || building.BuildingState == BuildingState.Plundered;
    }
}