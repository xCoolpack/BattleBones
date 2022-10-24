using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitTypeValue
{
    public UnitStrategicValues UnitValues;
    public static int DefaultEval = 100;

    public UnitTypeValue(UnitStrategicValues unitValues)
    {
        UnitValues = unitValues;
    }

    public int EvaluateUnitType(string unitName)
    {
        UnitValueRecord queriedValue = UnitValues.unitStrategicValues
                                        .SingleOrDefault(rec => rec.unitName == unitName);

        return queriedValue.unitValue is not 0
            ? queriedValue.unitValue
            : DefaultEval;
    }
}
