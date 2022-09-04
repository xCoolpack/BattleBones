using UnityEngine;

public class Building : MonoBehaviour
{
    // Max stats
    public int MaxHealth;
    public int MaxDefense;
    public readonly int BaseRepairCooldown; 

    // Current stats
    public int CurrentHealth;
    public int CurrentDefense;
    public int SightRange;
    public int CurrentRepairCooldown;

    // References 
    public BaseBuildingStats BaseBuildingStats;
    public Player Player;
    public Field Field;

    private void Awake()
    {
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Methods setting unit stats from BaseBuildingStats object
    /// </summary>
    private void SetStats()
    {
        MaxHealth = BaseBuildingStats.BaseHealth;
        CurrentHealth = BaseBuildingStats.BaseHealth;
        MaxDefense = BaseBuildingStats.BaseDefense;
        CurrentDefense = BaseBuildingStats.BaseDefense;
        SightRange = BaseBuildingStats.BaseSightRange;
    }

}
