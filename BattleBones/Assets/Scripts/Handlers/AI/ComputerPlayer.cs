using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ComputerPlayer : MonoBehaviour
{
    public Player playerComponent;
    public EvaluationEngine evaluationEngine;
    public int unitRecruitmentCooldown;

    void Awake()
    {
        playerComponent = GetComponent<Player>();
        unitRecruitmentCooldown = 3;
    }

    public void ProcessTurn()
    {
        SortPlayersEntities();

        foreach (Building building in playerComponent.Buildings.ToList())
        {
            List<Move> moves = GenerateMoves(building);
            SelectAndRunMoves(moves);

            if (building.BaseBuildingStats.BuildingName == "Outpost")
            {
                if (unitRecruitmentCooldown == 0)
                {
                    Outpost outpost = building.GetComponent<Outpost>();
                    moves = GenerateRecruitment(outpost, building.Player);
                    SelectAndRunMoves(moves, false);
                    unitRecruitmentCooldown = 3;
                }
                else
                {
                    unitRecruitmentCooldown--;
                }
            }
            //TO-DO: repair
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
        string[] defensiveNames = { "Defensive tower", "Outpost" };

        if (defensiveNames.Contains(buildingName))
        {
            DefensiveBuilding defensiveBuilding = building.GetComponent<DefensiveBuilding>();
            defensiveBuilding.SetAttackableFields();

            foreach (Field field in defensiveBuilding.AttackableFields)
            {
                moves.Add(new Move(evaluationEngine.Evaluate("buildingAttack", building, field),
                    () => { defensiveBuilding.Attack(field); }));
            }
            
        }

        //TO-DO: repairing buildings

        return moves;
    }

    public List<Move> GenerateRecruitment(Outpost outpost, Player player)
    {
        List<Move> moves = new List<Move>();

        foreach (Object obj in player.UnlockedUnits)
        {
            Unit unit = obj.GetComponent<Unit>();
            
            if (unit.CanAffordRecruitment(player))
            {
                moves.Add(new Move(evaluationEngine.Evaluate("recruitment", player, unit),
                    () => { outpost.RecruitUnit(obj as GameObject); } ));
            }
        }

        return moves;
    }

    public void SelectAndRunMoves(List<Move> moves, bool randomise = true)
    {
        if (moves.Count == 0)
            return;

        List<Move> toRandomise = moves.OrderByDescending(move => move.EvalValue).Take(3).ToList();
        
        if (randomise)
        {
            int sumEval = 0;

            foreach (Move move in toRandomise)
            {
                sumEval += System.Math.Abs(move.EvalValue);
            }

            int chosenEvalLevel = new System.Random().Next(0, sumEval - 1);
            while (chosenEvalLevel > 0)
            {
                chosenEvalLevel -= System.Math.Abs(toRandomise.First().EvalValue);
                if (chosenEvalLevel > 0)
                    toRandomise.RemoveAt(0);
            }
        }

        Move toExecute = toRandomise.First();

        toExecute.Execute();
    }
}
