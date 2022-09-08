using UnityEngine;

[CreateAssetMenu(fileName = "New BaseBuildingStats", menuName = "BaseBuildingStats")]
public class BaseBuildingStats : ScriptableObject
{
    public string BuildingName;
    public int BaseHealth;
    public int BaseDefense;
    public int BaseSightRange;

    public Resources BaseCost;
}
