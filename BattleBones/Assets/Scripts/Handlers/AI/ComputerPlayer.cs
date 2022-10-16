using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComputerPlayer : MonoBehaviour
{
    public Player playerComponent;
    public EvaluationEngine evaluationEngine;

    void Awake()
    {
        playerComponent = GetComponent<Player>();
        evaluationEngine = new EvaluationEngine();
    }

    public void ProcessTurn()
    {
        SortPlayersEntities();

        foreach (Building building in playerComponent.Buildings)
        {
            List<Move> moves = GenerateMoves(building);
            SelectAndRunMoves(moves);
        }

        foreach (Unit unit in playerComponent.Units)
        {
            List<Move> moves = GenerateMoves(unit);
            SelectAndRunMoves(moves);
        }

        playerComponent.TurnHandler.NextTurn();
    }

    public void SortPlayersEntities()
    {
        // desc sort with ranged units going first (only cat units for now)
        playerComponent.Units.Sort((b, a) =>
        {
            string[] rangedNames = {"Crossbowcat"};
            string nameA = a.BaseUnitStats.UnitName;
            string nameB = b.BaseUnitStats.UnitName;

            if (nameA == nameB
                || (rangedNames.Contains(nameA) && rangedNames.Contains(nameB)))
                    return 0;

            if (rangedNames.Contains(nameB) && !rangedNames.Contains(nameA))
                return -1;

            return 1;
        });
    }

    public List<Move> GenerateMoves(Object entity)
    {
        return entity is Unit ? 
            GenerateUnitMoves((Unit) entity) 
            : GenerateBuildingMoves((Building) entity);
    }

    public List<Move> GenerateUnitMoves(Unit unit)
    {
        List<Move> moves = new List<Move>();
        unit.UpdateFieldSets();
        List<Field> attackableFields = unit.AttackableFields;
        //TO-DO: consider defending/healing

        //for now units always attack if possible (this might become permanent)
        //TO-DO: consider retreating (but probably ignore it)
        if (attackableFields.Count == 0)
        {
            List<Field> moveableFields = unit.MoveableFields;

            foreach (Field field in moveableFields)
            {
                moves.Add(new Move(evaluationEngine.Evaluate("move", unit, field), 
                    () => { unit.Move(field); }));
            }
        }

        foreach (Field field in attackableFields)
        {
            moves.Add(new Move(evaluationEngine.Evaluate("move", unit, field), 
                () => { unit.Attack(field); }));
        }

        return moves;
    }

    public List<Move> GenerateBuildingMoves(Building building)
    {
        List<Move> moves = new List<Move>();

        if (building.BaseBuildingStats.BuildingName == "DefensiveTower")
        {
            //TO-DO: how to get defensive building
        }

        if (building.BaseBuildingStats.BuildingName != "Outpost")
        {
            //TO-DO: recruitment
        }

        return moves;
    }

    public void SelectAndRunMoves(List<Move> moves)
    {
        if (moves.Count == 0)
            return;

        moves.OrderByDescending(move => move.EvalValue);

        //TO-DO: introduce randomisation
        moves[0].Execute();
    }
}
