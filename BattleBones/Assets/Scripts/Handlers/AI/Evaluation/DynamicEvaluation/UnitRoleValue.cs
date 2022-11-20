using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRoleValue
{
    int desiredUnits = 3;

    public UnitRoleValue()
    {

    }

    public double EvaluateUnitRoleValue(Unit target, Player player)
    {
        int sameUnitCount = player.Units
            .FindAll(x => x.BaseUnitStats.UnitName == target.BaseUnitStats.UnitName).Count;

        return desiredUnits / (sameUnitCount + 1);
    }
}
