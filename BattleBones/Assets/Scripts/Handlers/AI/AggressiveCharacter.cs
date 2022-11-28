using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveCharacter : CustomEvaluationCharacteristics
{
    public virtual int ProcessMovement(Unit source, Field target, int eval)
    {
        return eval;
    }
}
