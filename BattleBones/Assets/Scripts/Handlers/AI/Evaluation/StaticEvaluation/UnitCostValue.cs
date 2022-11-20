using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCostValue
{
    public double goldCoeff = 0.2;
    public double woodCoeff = 0.1;
    public double stoneCoeff = 0.15;
    public double doggiumCoeff = 0.3;
    public double boneCoeff = 0.5;

    public UnitCostValue()
    {

    }

    public double EvaluateUnitCostValue(Unit target)
    {
        Resources unitCost = target.BaseUnitStats.BaseCost;
        double ucv = unitCost.Gold * goldCoeff
            + unitCost.Wood * woodCoeff
            + unitCost.Stone * stoneCoeff
            + unitCost.Doggium * doggiumCoeff
            + unitCost.Bone * boneCoeff;

        return ucv;
    }
}
