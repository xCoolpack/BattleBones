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
    public readonly int BaseRepairCooldown = 1; 

    // Current stats
    public int CurrentHealth;
    public int SightRange;
    public int CurrentRepairCooldown;

    // References 
    public BaseBuildingStats BaseBuildingStats;
    public Player Player;
    public Field Field;
    public BuildingState BuildingState;

    private void Awake()
    {
        SetStats();
    }

    /// <summary>
    /// Methods setting unit stats from BaseBuildingStats object
    /// </summary>
    private void SetStats()
    {
        MaxHealth = BaseBuildingStats.BaseHealth;
        CurrentHealth = BaseBuildingStats.BaseHealth;
        SightRange = BaseBuildingStats.BaseSightRange;
    }

    public bool IsEnemy(Player player)
    {
        return Player != player;
    }

    public bool CanAffordConstruction(Player player)
    {
        return player.ResourceManager.ResourcesAmount >= BaseBuildingStats.BaseCost;
    }

    public bool CanAffordRepair()
    {
        return Player.ResourceManager.ResourcesAmount >= BaseBuildingStats.BaseCost / 2;
    }


    public UnitModifiers GetUnitModifiers()
    {
        return GetComponent<Barricade>()?.BuildingUnitModifier ?? new UnitModifiers();
    }

    public void Construct()
    {
        BuildingState = BuildingState.Fine;
        Field.Unit?.AddUnitModifiers(Field.Building.GetUnitModifiers());
    }

    public void Plunder()
    {
        BuildingState = BuildingState.Plundered;
        Field.Unit?.RemoveUnitModifiers(Field.Building.GetUnitModifiers());
    }

    public bool CanRepair()
    {
        return CanAffordRepair();
    }

    public void BeginRepair()
    {
        BuildingState = BuildingState.UnderRepair;
        Player.ResourceManager.RemoveAmount(BaseBuildingStats.BaseCost/2);
        Player.PlayerEventHandler.AddStartTurnEvent(new GameEvent(BaseRepairCooldown, Repair));
    }

    public void Repair()
    {
        BuildingState = BuildingState.Fine;
        Field.Unit?.AddUnitModifiers(Field.Building.GetUnitModifiers());
    }

    public void Destroy()
    {
        Field.Unit?.RemoveUnitModifiers(Field.Building.GetUnitModifiers());
        Field.Building = null;
        Destroy(gameObject);
    }

    public void HandleOnClick()
    {
        Debug.Log("Building click");
    }
}
