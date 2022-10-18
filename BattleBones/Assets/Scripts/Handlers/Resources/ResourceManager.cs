using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Resources ResourcesAmount;

    public Resources ResourcesIncome;

    public void GenerateIncome()
    {
        ResourcesAmount += ResourcesIncome;
    }

    public void AddIncome(Resources res)
    {
        ResourcesIncome += res;
    }

    public void AddAmount(Resources res)
    {
        ResourcesAmount += res;
    }

    public void RemoveIncome(Resources res)
    {
        ResourcesIncome -= res;
    }

    public void RemoveAmount(Resources res)
    {
        ResourcesAmount -= res;
    }
}

