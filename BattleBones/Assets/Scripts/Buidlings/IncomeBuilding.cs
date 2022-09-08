using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeBuilding : MonoBehaviour
{
    public Resources ResourcesIncome;
    public Building Building;

    private void Awake()
    {
        Building = GetComponent<Building>();
    }

    public void Construct()
    {
        Building.Construct();
        Building.Player.ResourceManager.ResourcesIncome += ResourcesIncome;
    }

    public void Plunder()
    {
        Building.Plunder();
        Building.Player.ResourceManager.ResourcesIncome -= ResourcesIncome;
    }

    public void Destroy()
    {
        Building.Player.ResourceManager.ResourcesIncome -= ResourcesIncome;
        Building.Destroy();
    }
}
