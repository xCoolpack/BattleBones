using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TurnHandler TurnHandler;
    public int MaxUnitCap;
    public int CurrentUnitCap => Units.Count;
    public int TradeBuildingCounter;
    public string Faction;
    public ResourceManager ResourceManager;
    public MissionHandler MissionHandler;
    public EventHandler PlayerEventHandler;
    public List<GameObject> UnlockedUnits; // It has to be GameObject because we need Instantiate that unit
    public List<GameObject> AvailableBuildings; // It has to be GameObject because we need Instantiate that building
    public Dictionary<string, UnitModifiers> UnitModifiersDictionary; 

    public List<Unit> Units;
    public List<Building> Buildings;

    public Overlay Overlay;

    public void AddUnit(Unit unit)
    {
        Units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        Units.Remove(unit);
    }

    public void AddUnitCap(int cap)
    {
        MaxUnitCap += cap;
    }

    public void RemoveUnitCap(int cap)
    {
        MaxUnitCap -= cap;
    }

    public bool HaveEnoughUnitCap()
    {
        return CurrentUnitCap < MaxUnitCap;
    }

    public void RestoreUnitsMovementPoints()
    {
        Units.ForEach(u => u.RestoreMovementPoints());
    }

    public void ApplyUnitsModifiers()
    {
        Units.ForEach(u => u.SetCurrentStats());
    }

    public void AddBuilding(Building building)
    {
        Buildings.Add(building);
    }

    public void RemoveBuilding(Building building)
    {
        Buildings.Remove(building);
    }

    public bool IsPlayersTurn()
    {
        return TurnHandler.IsPlayersTurn(this);
    }
}
