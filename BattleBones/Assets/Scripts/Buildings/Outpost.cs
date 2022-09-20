using Microsoft.Cci;
using System.Linq;
using UnityEngine;

public class Outpost : MonoBehaviour
{
    public GameEvent RecruitingUnit;
    public Building Building { get; private set; }

    private void Awake()
    {
        Building = GetComponent<Building>();
    }

    private void OnMouseDown()
    {
        if (RecruitingUnit != null)
            CancelRecruitment();
        else
            BeginUnitRecruitment("TestUnit");
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
        Building.Field.Unit = unit;
        Building.Player.AddUnit(unit);

        RecruitingUnit = null;
    }

    public void CancelRecruitment()
    {
        EventHandler eventHandler = Building.Player.PlayerEventHandler;
        eventHandler.RemoveStartTurnEvent(RecruitingUnit);
        RecruitingUnit = null;
    }
}
