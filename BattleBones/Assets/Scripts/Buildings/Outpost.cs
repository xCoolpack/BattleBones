using Microsoft.Cci;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

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
        GameObject unitPrefab = Building.Player.UnlockedUnits.FirstOrDefault(g => g.name == $"{Building.Player.Faction} {unitName}");
        if (unitPrefab is null)
            return false;
        return !IsUnitBeingRecruited() && unitPrefab.GetComponent<Unit>().CanAffordRecruitment(Building.Player) && 
               !Building.Field.HasUnit() && Building.Player.HaveEnoughUnitCap();
    }

    public void BeginUnitRecruitment(string unitName)
    {
        EventHandler eventHandler = Building.Player.PlayerEventHandler;
        GameObject unitPrefab = Building.Player.UnlockedUnits.FirstOrDefault(g => g.name == $"{Building.Player.Faction} {unitName}");
        RecruitingUnit = new GameEvent(1, () => RecruitUnit(unitPrefab));
        _unitCost = unitPrefab.GetComponent<Unit>().BaseUnitStats.BaseCost;
        Building.Player.ResourceManager.RemoveAmount(_unitCost);
        eventHandler.AddStartTurnEvent(RecruitingUnit);
    }

    /// <summary>
    /// Methods responsible for creating gameObject with unit script
    /// </summary>
    /// <param name="unitPrefab"></param>
    public void RecruitUnit(GameObject unitPrefab)
    {
        var tran = Building.Field.transform;
        Unit unit = Instantiate(unitPrefab, new Vector3(tran.position.x, tran.position.y + 0.4f, 0), Quaternion.identity, tran)
            .GetComponent<Unit>();
        unit.Player = Building.Player;
        unit.Field = Building.Field;
        unit.CurrentModifiers = Building.Player.UnitModifiersDictionary[unit.BaseUnitStats.UnitName];
        unit.SetCurrentStats();
        unit.SetStartingStats();
        Building.Field.Unit = unit;
        Building.Player.AddUnit(unit);
        RecruitingUnit = null;

        // Set unit visibility
        foreach (Player key in Building.Field.SeenBy.Keys)
        {
            unit.Show(key);
        }

        Logger.Log($"{Building.Player.name} has finished recruitment of {unit.BaseUnitStats.UnitName} at {Building.Field.ThreeAxisCoordinates}");

        unit.ShowFields(unit.Field);
    }

    public bool IsUnitBeingRecruited()
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
