using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;

public class Mission3SideObjective2 : MonoBehaviour, IObjective
{
    [SerializeField] private List<Field> _fieldList;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $"Towers destroyed: {_fieldList.Sum(CheckTower)}/{_fieldList.Count}";

    private bool CompletionCheck()
    {
        return _fieldList.Sum(CheckTower) >= _fieldList.Count;
    }

    private int CheckTower(Field field)
    {
        return (field.HasBuilding() && field.Building.BaseBuildingStats.BuildingName == "Defensive tower") ? 0 : 1;
    }
}
