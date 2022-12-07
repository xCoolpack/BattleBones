using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission0PrimaryObjective : MonoBehaviour, IObjective
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
        return _enemyPlayer.Buildings.FirstOrDefault(building => building.gameObject.GetComponent<Outpost>() != null) == null;
    }
}
