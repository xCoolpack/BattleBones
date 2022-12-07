using System.Linq;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission3PrimaryObjective : MonoBehaviour, IObjective
{
    [SerializeField]
    private Player _enemyPlayer;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $"Destroy enemy outpost";

    private bool CompletionCheck()
    {
        var building =
            _enemyPlayer.Buildings.FirstOrDefault(building => building.gameObject.GetComponent<Outpost>() != null);
        return building == null || building.BuildingState == BuildingState.Plundered;
    }
}
