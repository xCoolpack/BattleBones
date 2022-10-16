using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Mission1PrimaryObjective : MonoBehaviour, IObjective
{
    [SerializeField]
    private Player _enemyPlayer;

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
            _enemyPlayer.Buildings.FirstOrDefault(building => building.gameObject.GetComponent<Outpost>() != null);
        return building == null || building.BuildingState == BuildingState.Plundered;
    }
}
