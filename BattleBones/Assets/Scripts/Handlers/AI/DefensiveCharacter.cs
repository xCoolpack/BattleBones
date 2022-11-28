using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefensiveCharacter : CustomEvaluationCharacteristics
{
    public PlayerBaseDistance PlayerBaseDistance;

    public DefensiveCharacter()
    {
        PlayerBaseDistance = new PlayerBaseDistance();
    }

    public int ProcessMovement(Unit source, Field target, int eval)
    {
        Field friendlyBase = UnitRelatedEvaluation.GetAiBase(source.Player, target);

        if (target.Coordinates == friendlyBase.Coordinates)
            return -1000;

        return PlayerBaseDistance.EvaluateDistance(target, friendlyBase);
    }
}
