using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    public readonly UnitModifiers BuildingUnitModifier = new (defense: 15);

    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
    }

}
