using System.Linq;
using UnityEngine;

public class Mission3SideObjective2 : MonoBehaviour, IObjective
{
    [SerializeField]
    private Field _field1;
    [SerializeField]
    private Field _field2;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $"Towers destroyed: {CheckTower(_field1) + CheckTower(_field2)}/2";

    private bool CompletionCheck()
    {
        return CheckTower(_field1) + CheckTower(_field2) >= 2;
    }

    private int CheckTower(Field field)
    {
        return (field.HasBuilding() && field.Building.BaseBuildingStats.BuildingName == "Defensive tower") ? 0 : 1;
    }
}
