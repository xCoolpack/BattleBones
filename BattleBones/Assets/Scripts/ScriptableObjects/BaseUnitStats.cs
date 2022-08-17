using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BaseUnitStats", menuName = "BaseUnitStats")]
public class BaseUnitStats : ScriptableObject
{
    public string unitName;
    public float baseHealth;
    public float baseDamage;
    public float baseDefense;
    public float baseMovementPoints;
    public float baseAttackRange;
    public float baseSightRange;

    public ResourceCost baseCost;
}
