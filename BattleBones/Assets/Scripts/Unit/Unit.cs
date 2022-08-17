using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Max Stats
    public float maxHealth;
    public float maxDamage;
    public float maxDefense;
    public float maxMovementPoints;

    // Current Stats
    public float currentHealth;
    public float currentDamage;
    public float currentDefense;
    public float currentMovementPoints;
    public float attackRange;
    public float sightRange;

    // References
    public BaseUnitStats baseUnitStats;
    public GameObject Player;
    public Field field;

    private void Awake()
    {
        SetStats();
    }

    private void Update()
    {


    }
    private void OnMouseDown()
    {
        Debug.Log("clicked");
    }

    /// <summary>
    /// Methods setting unit stats from BaseUnitStats object
    /// </summary>
    private void SetStats()
    {
        maxHealth = baseUnitStats.baseHealth;
        currentHealth = baseUnitStats.baseHealth;
        maxDamage = baseUnitStats.baseDamage;
        currentDamage = baseUnitStats.baseDamage;
        maxDefense = baseUnitStats.baseDefense;
        currentDefense = baseUnitStats.baseDefense;
        maxMovementPoints = baseUnitStats.baseMovementPoints;
        currentMovementPoints = baseUnitStats.baseMovementPoints;
        attackRange = baseUnitStats.baseAttackRange;
        sightRange = baseUnitStats.baseSightRange;
    }

    

}

