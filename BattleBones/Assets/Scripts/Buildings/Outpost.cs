using Microsoft.Cci;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Outpost : MonoBehaviour
{
    public GameEvent RecruitingUnit;
    private Resources _unitCost;
    public Building Building { get; private set; }

    private void Awake()
    {
        Building = GetComponent<Building>();
    }

    private void OnMouseDown()
    {
        //if (IsUnitBeingRecruited())
        //    CancelRecruitment();
        //else
        //    BeginUnitRecruitment("TestUnit");
    }

    public bool CanRecruit(string unitName)
    {
        GameObject unitPrefab = Building.Player.UnlockedUnits.FirstOrDefault(g => g.name == unitName);
        if (unitPrefab is null)
            return false;
        return !IsUnitBeingRecruited() && unitPrefab.GetComponent<Unit>().CanAffordRecruitment(Building.Player) && 
               !Building.Field.HasUnit() && Building.Player.HaveEnoughUnitCap();
    }

    public void BeginUnitRecruitment(string unitName)
    {
        EventHandler eventHandler = Building.Player.PlayerEventHandler;
        RecruitingUnit = new GameEvent(1, () => RecruitUnit(unitName));
        eventHandler.AddStartTurnEvent(RecruitingUnit);

        Logger.Log($"{Building.Player.name} has begun recruitment of {unitName} at {Building.Field.ThreeAxisCoordinates}");
    }

    /// <summary>
    /// Methods responsible for creating gameObject with unit script
    /// </summary>
    /// <param name="unitName"></param>
    public void RecruitUnit(string unitName)
    {
        GameObject unitPrefab = Building.Player.UnlockedUnits.FirstOrDefault(g => g.name == unitName);
        Unit unit = Instantiate(unitPrefab, Building.Field.transform).GetComponent<Unit>();
        unit.Player = Building.Player;
        unit.Field = Building.Field;
        unit.CurrentModifiers = Building.Player.UnitModifiersDictionary[unitName];
        unit.SetCurrentStats();
        unit.SetStartingStats();
        Building.Field.Unit = unit;
        Building.Player.AddUnit(unit);
        _unitCost = unit.BaseUnitStats.BaseCost;
        Building.Player.ResourceManager.RemoveAmount(_unitCost);

        // Set unit visibility
        foreach (Player key in Building.Field.SeenBy.Keys)
        {
            unit.Show(key);
        }

        Logger.Log($"{Building.Player.name} has finished recruitment of {unitName} at {Building.Field.ThreeAxisCoordinates}");

        unit.ShowFields(unit.Field);
    }

    public bool IsUnitBeingRecruited()
    {
        return RecruitingUnit != null;
    }

    public void CancelRecruitment()
    {
        Logger.Log($"{Building.Player.name} has cancel recruitment at {Building.Field.ThreeAxisCoordinates}");

        EventHandler eventHandler = Building.Player.PlayerEventHandler;
        eventHandler.RemoveStartTurnEvent(RecruitingUnit);
        Building.Player.ResourceManager.AddAmount(_unitCost);
        RecruitingUnit = null;
    }
}
