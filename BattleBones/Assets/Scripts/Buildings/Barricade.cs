using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    public UnitModifiers BuildingUnitModifier;

    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
    }

}
