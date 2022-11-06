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
    }

    public void ProcessTurn()
    {
        SortPlayersEntities();

        foreach (Building building in playerComponent.Buildings.ToList())
        {
            List<Move> moves = GenerateMoves(building);
            SelectAndRunMoves(moves);
            //TO-DO: recruitment and repair
        }

        foreach (Unit unit in playerComponent.Units.ToList())
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
        //TO-DO: building construction
        return entity is Unit ? 
            GenerateUnitMoves((Unit) entity) 
            : GenerateBuildingMoves((Building) entity);
    }

    public List<Move> GenerateUnitMoves(Unit unit)
    {
        List<Move> moves = new List<Move>();
        unit.UpdateFieldSets();
        List<Field> attackableFields = unit.AttackableFields;

        //units always attack if possible
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
            moves.Add(new Move(evaluationEngine.Evaluate("unitAttack", unit, field), 
                () => { unit.Attack(field); }));
        }

        return moves;
    }

    public List<Move> GenerateBuildingMoves(Building building)
    {
        List<Move> moves = new List<Move>();
        string buildingName = building.BaseBuildingStats.BuildingName;


        if (building.BaseBuildingStats.BuildingName == "Defensive tower")
        {
            DefensiveBuilding defensiveBuilding = GetComponent<DefensiveBuilding>();
            defensiveBuilding.SetAttackableFields();
            // moveName = buildingAttack
            foreach (Field field in defensiveBuilding.AttackableFields)
            {
                //TO-DO: add atacking when defBuilding has Attack method
            }
            
        }

        //TO-DO: repairing buildings and construction

        return moves;
    }

    public List<Move> GenerateRecruitment(Building building)
    {
        //TO-DO
        // moveName = recruitment
        return null;
    }

    public void SelectAndRunMoves(List<Move> moves)
    {
        if (moves.Count == 0)
            return;

        Move toExecute = moves.OrderByDescending(move => move.EvalValue).First();

        //TO-DO: introduce randomisation
        toExecute.Execute();
    }
}
