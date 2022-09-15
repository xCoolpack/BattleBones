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
        ResourcesAmount += res;
    }

    public void RemoveIncome(Resources res)
    {
        ResourcesAmount -= res;
    }
}

