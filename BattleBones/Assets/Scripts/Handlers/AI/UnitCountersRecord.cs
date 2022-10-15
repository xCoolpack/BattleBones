using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitCounterValue
{
    public string unitAttacking;
    public int counterValue;
}

[System.Serializable]
public struct UnitCountersRecord
{
    public string unitDefending;
    public List<UnitCounterValue> counters;
}
