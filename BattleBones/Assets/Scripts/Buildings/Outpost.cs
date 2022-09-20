using Microsoft.Cci;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Outpost : MonoBehaviour
{
    public GameEvent RecruitingUnit;
    private Resources _unitCost;
    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
    }

    private void OnMouseDown()
    {
        if (CanCancelRecruitment())
            CancelRecruitment();
        else
            BeginUnitRecruitment("TestUnit");
    }

    public bool CanRecruit(string unitName)
    {
        GameObject unitPrefab = _building.Player.UnlockedUnits.FirstOrDefault(g => g.name == unitName);
        if (unitPrefab is null)
            return false;
        return unitPrefab.GetComponent<Unit>().CanAffordRecruitment(_building.Player) && !_building.Field.HasUnit();
    }

    public void BeginUnitRecruitment(string unitName)
    {
        EventHandler eventHandler = _building.Player.PlayerEventHandler;
        RecruitingUnit = new GameEvent(1, () => RecruitUnit(unitName));
        eventHandler.AddStartTurnEvent(RecruitingUnit);
    }

    /// <summary>
    /// Methods responsible for creating gameObject with unit script
    /// </summary>
    /// <param name="unitName"></param>
    public void RecruitUnit(string unitName)
    {
        GameObject unitPrefab = _building.Player.UnlockedUnits.FirstOrDefault(g => g.name == unitName);
        Unit unit = Instantiate(unitPrefab, _building.Field.transform).GetComponent<Unit>();
        unit.Player = _building.Player;
        unit.Field = _building.Field;
        unit.CurrentModifiers = _building.Player.UnitModifiersDictionary[unitName];
        unit.SetCurrentStats();
        _building.Field.Unit = unit;
        _building.Player.AddUnit(unit);
        _unitCost = unit.BaseUnitStats.BaseCost;
        _building.Player.ResourceManager.RemoveAmount(_unitCost);
    }

    public bool CanCancelRecruitment()
    {
        return RecruitingUnit != null;
    }

    public void CancelRecruitment()
    {
        EventHandler eventHandler = _building.Player.PlayerEventHandler;
        eventHandler.RemoveStartTurnEvent(RecruitingUnit);
        _building.Player.ResourceManager.AddAmount(_unitCost);
        RecruitingUnit = null;
    }
}
