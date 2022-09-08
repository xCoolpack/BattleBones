using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeBuilding : Building
{
    public Resources ResourcesIncome;

    public override void Construct()
    {
        base.Construct();
        Player.ResourceManager.ResourcesIncome += ResourcesIncome;
    }

    public override void Plunder()
    {
        base.Plunder();
        Player.ResourceManager.ResourcesIncome -= ResourcesIncome;
    }

    public override void Destroy()
    {
        Player.ResourceManager.ResourcesIncome -= ResourcesIncome;
        base.Destroy();
    }
}
