using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission4SideObjective1 : MonoBehaviour, IObjective
{
    [SerializeField] private Player _player;
    [SerializeField] private int _unitCount;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $"Current units: {_player.CurrentUnitCap}/{_unitCount}";

    private bool CompletionCheck()
    {
        return _player.CurrentUnitCap >= _unitCount;
    }
}
