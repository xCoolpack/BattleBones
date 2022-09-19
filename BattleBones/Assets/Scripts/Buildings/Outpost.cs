using Microsoft.Cci;
using System.Linq;
using UnityEngine;

public class Outpost : MonoBehaviour
{
    public GameEvent RecruitingUnit;
    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
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
        _building.Player.UnlockedUnits.ForEach(e => Debug.Log(e.name));
        Debug.Log(unitPrefab);
        Unit unit = Instantiate(unitPrefab, _building.Field.transform).GetComponent<Unit>();
        unit.Player = _building.Player;
        unit.Field = _building.Field;
        _building.Field.Unit = unit;
        _building.Player.AddUnit(unit);

        RecruitingUnit = null;
    }

    public void CancelRecruitment()
    {
        EventHandler eventHandler = _building.Player.PlayerEventHandler;
        eventHandler.RemoveStartTurnEvent(RecruitingUnit);
        RecruitingUnit = null;
    }
}
