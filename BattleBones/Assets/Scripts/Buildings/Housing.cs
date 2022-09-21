using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Housing : MonoBehaviour
{
    public readonly int UnitCap = 3;
    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
    }

    public void Construct()
    {
        _building.Player.AddUnitCap(UnitCap);
    }

    public void Plunder()
    {
        _building.Player.RemoveUnitCap(UnitCap);
    }

    public void Repair()
    {
        _building.Player.AddUnitCap(UnitCap);
    }

    public void Destroy()
    {
        _building.Player.RemoveUnitCap(UnitCap);
    }
}
