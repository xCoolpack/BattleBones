using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission4FailObjective : MonoBehaviour, IObjective
{
    [SerializeField]
    private Player _player;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $@"Current Unit Count: {_player.CurrentUnitCap}";

    private bool CompletionCheck()
    {
        return _player.CurrentUnitCap <= 0;
    }
}