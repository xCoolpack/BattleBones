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
                processedEval = ProcessUnitAttack(source as Unit, target as Field, eval);
                break;

            case "move":
                processedEval = ProcessMovement(source as Unit, target as Field, eval);
                break;

            case "buildingAttack":
                processedEval = ProcessBuildingAttack(target as Field, eval);
                break;

            case "recruitment":
                processedEval = ProcessRecruitment(target as Unit, eval);
                break;

            case "building":
                processedEval = ProcessBuilding(source as Building, target as Field, eval);
                break;

            default:
                break;
        }

        return processedEval;
    }

    public int ProcessUnitAttack(Unit source, Field target, int eval)
    {
        return eval;
    }

    public int ProcessMovement(Unit source, Field target, int eval);

    public int ProcessBuildingAttack(Field target, int eval)
    {
        return eval;
    }

    public int ProcessRecruitment(Unit target, int eval)
    {
        return eval;
    }

    public int ProcessBuilding(Building source, Field target, int eval)
    {
        return eval;
    }
}
