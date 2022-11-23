using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FieldType", menuName = "FieldType")]
public class FieldType : ScriptableObject
{
    public string FieldName;
    public int MovementPointsCost;
    public bool IsObstacle;
    public bool IsBlockingSight;

    public UnitModifiers FieldUnitModifiers;
    public List<BaseBuildingStats> AllowableBuildings;
}
