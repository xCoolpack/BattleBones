using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComputerPlayer : MonoBehaviour
{
    public static Player playerComponent;
    public EvaluationEngine evaluationEngine;

    void Awake()
    {
        playerComponent = GameObject.Find("Player2").GetComponent<Player>();
    }

    void Update()
    {
        if (playerComponent.IsPlayersTurn())
            ProcessTurn();
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
        
        //end turn?
    }

    public void SortPlayersEntities()
    {
        // desc sort with ranged units going first
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
        List<Field> attackableFields = unit.AttackableFields;

        //TO-DO: consider defending/healing

        //for now units always attack if possible (this might become permanent)
        //TO-DO: consider retreating (but probably ignore it)
        if (attackableFields.Count == 0)
        {
            List<Field> moveableFields = unit.MoveableFields;

            foreach (Field field in moveableFields)
            {
                moves.Add(new Move(evaluationEngine.evaluate("move", unit, field), 
                    () => { unit.Move(field); }));
            }
        }

        foreach (Field field in attackableFields)
        {
            moves.Add(new Move(evaluationEngine.evaluate("move", unit, field), 
                () => { unit.Attack(field); }));
        }

        return moves;
    }

    public List<Move> GenerateBuildingMoves(Building building)
    {
        //TO-DO
        return null;
    }

    public void SelectAndRunMoves(List<Move> moves)
    {
        if (moves.Count == 0)
            return;
    }
}
