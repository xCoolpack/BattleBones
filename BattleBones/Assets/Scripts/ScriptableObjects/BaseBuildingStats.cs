using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BaseBuildingStats", menuName = "BaseBuildingStats")]
public class BaseBuildingStats : ScriptableObject
{
    public string BuildingName;
    public int BaseHealth;
    public int BaseDamage;
    public int BaseAttackRange;
    public int BaseSightRange;
    public bool IsPassable;
    
    public Resources BaseCost;
}
