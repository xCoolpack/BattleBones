using UnityEngine;

[CreateAssetMenu(fileName = "New BaseUnitStats", menuName = "BaseUnitStats")]
public class BaseUnitStats : ScriptableObject
{
    public string unitName;
    public int baseHealth;
    public int baseDamage;
    public int baseDefense;
    public int baseMovementPoints;
    public int baseAttackRange;
    public int baseSightRange;

    public ResourceCost baseCost;
}
