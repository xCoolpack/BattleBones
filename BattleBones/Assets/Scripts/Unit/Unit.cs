using UnityEngine;

public class Unit : MonoBehaviour
{
    // Max Stats
    public int maxHealth;
    public int maxDamage;
    public int maxDefense;
    public int maxMovementPoints;

    // Current Stats
    public int currentHealth;
    public int currentDamage;
    public int currentDefense;
    public int currentMovementPoints;
    public int attackRange;
    public int sightRange;

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

