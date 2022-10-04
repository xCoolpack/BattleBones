using Microsoft.Cci;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        //if (CanCancelRecruitment())
        //    CancelRecruitment();
        //else
        //    BeginUnitRecruitment("TestUnit");
    }

    public bool CanRecruit(string unitName)
    {
        GameObject unitPrefab = Building.Player.UnlockedUnits.FirstOrDefault(g => g.name == unitName);
        if (unitPrefab is null)
            return false;
        return unitPrefab.GetComponent<Unit>().CanAffordRecruitment(Building.Player) && 
               !Building.Field.HasUnit() && Building.Player.HaveEnoughUnitCap();
    }

    public void BeginUnitRecruitment(string unitName)
    {
        EventHandler eventHandler = Building.Player.PlayerEventHandler;
        RecruitingUnit = new GameEvent(1, () => RecruitUnit(unitName));
        eventHandler.AddStartTurnEvent(RecruitingUnit);
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
    }

    public bool CanCancelRecruitment()
    {
        return RecruitingUnit != null;
    }

    public void CancelRecruitment()
    {
        EventHandler eventHandler = Building.Player.PlayerEventHandler;
        eventHandler.RemoveStartTurnEvent(RecruitingUnit);
        Building.Player.ResourceManager.AddAmount(_unitCost);
        RecruitingUnit = null;
    }
}
