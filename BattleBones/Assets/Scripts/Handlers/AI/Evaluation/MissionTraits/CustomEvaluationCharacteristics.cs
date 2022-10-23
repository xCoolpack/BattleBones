using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CustomEvaluationCharacteristics
{
    public static int DefaultCustomEval = 0;

    public int ProcessEvaluation(string moveType, Object source, Object target, int eval)
    {
        int processedEval = eval;

        switch (moveType)
        {
            case "unitAttack":
                processedEval = ProcessUnitAttack(source as Unit, target as Field);
                break;

            case "move":
                processedEval = ProcessMovement(target as Field);
                break;

            case "buildingAttack":
                processedEval = ProcessBuildingAttack(target as Field);
                break;

            case "recruitment":
                processedEval = ProcessRecruitment(target as Unit);
                break;

            case "building":
                processedEval = ProcessBuilding(source as Building, target as Field);
                break;

            default:
                break;
        }

        return processedEval;
    }

    public int ProcessUnitAttack(Unit source, Field target);

    public int ProcessMovement(Field target);

    public int ProcessBuildingAttack(Field target);

    public int ProcessRecruitment(Unit target);

    public int ProcessBuilding(Building source, Field target);
}
