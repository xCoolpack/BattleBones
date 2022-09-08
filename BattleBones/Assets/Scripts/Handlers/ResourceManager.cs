using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Resources ResourcesAmount;

    public Resources ResourcesIncome;
    
    public void GenerateIncome()
    {
        ResourcesAmount += ResourcesIncome;
    }
}

