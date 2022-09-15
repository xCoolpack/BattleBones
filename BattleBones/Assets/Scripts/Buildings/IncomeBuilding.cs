using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeBuilding : MonoBehaviour
{
    public Resources ResourcesIncome;
    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
    }

    public void Construct()
    {
        _building.Construct();
        _building.Player.ResourceManager.AddIncome(ResourcesIncome);
    }

    public void Plunder()
    {
        _building.Plunder();
        _building.Player.ResourceManager.RemoveIncome(ResourcesIncome);
    }

    public void Repair()
    {
        _building.Repair();
        _building.Player.ResourceManager.AddIncome(ResourcesIncome);
    }

    public void Destroy()
    {
        _building.Player.ResourceManager.RemoveIncome(ResourcesIncome);
        _building.Destroy();
    }
}
