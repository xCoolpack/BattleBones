using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Baricade : MonoBehaviour
{
    public int DefenseModifier;

    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
    }

    public void IncreaseDefense(Unit unit)
    {
        
    }
}
