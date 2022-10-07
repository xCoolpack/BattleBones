using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CounterValues", menuName = "CounterValues")]
public class CounterValues : ScriptableObject
{
    // counterValues[UnitDefending][UnitAttacking] => value how much the attacking unit counters the defending unit
    // high values mean that the attacking unit is dealing extra damage
    public List<UnitCountersRecord> counterValues;
}
