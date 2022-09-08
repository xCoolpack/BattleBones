using Unity.VisualScripting;
using UnityEngine;

public enum BuildingState
{
    UnderConstruction,
    Fine,
    Plundered,
    UnderRepair,
} 


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
    public BuildingState BuildingState;

    private void Awake()
    {
        Debug.Log("Building " +BaseBuildingStats);
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

    public bool IsEnemy(Player player)
    {
        return Player != player;
    }

    public void BeginConstruction(Player player, Field field)
    {
        Player = player;
        Field = field;
        field.Building = this;
        BuildingState = BuildingState.UnderConstruction;
        Player.PlayerEventHandler.AddStartTurnEvent(new GameEvent(1, Construct));
    }
    public void Construct()
    {
        BuildingState = BuildingState.Fine;
    }

    public void Plunder()
    {
        BuildingState = BuildingState.Plundered;
    }

    public void BeginRepair()
    {
        BuildingState = BuildingState.UnderRepair;
        Player.PlayerEventHandler.AddStartTurnEvent(new GameEvent(1, Repair));
    }

    public void Repair()
    {
        BuildingState = BuildingState.Fine;
    }

    public void Destroy()
    {
        Field.Building = null;
    }
}
