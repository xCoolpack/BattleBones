using UnityEngine;

[CreateAssetMenu(fileName = "New BaseUnitStats", menuName = "BaseUnitStats")]
public class BaseUnitStats : ScriptableObject
{
    public string UnitName;
    public int BaseHealth;
    public int BaseDamage;
    public int BaseDefense;
    public int BaseMovementPoints;
    public int BaseAttackRange;
    public int BaseSightRange;

    public ResourceCost BaseCost;
}
