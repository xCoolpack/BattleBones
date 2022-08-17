using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ResourceCost
{
    public float goldCost;
    public float woodCost;
    public float stoneCost;
    public float doggiumCost;

    public ResourceCost(float goldCost, float woodCost, float stoneCost, float doggiumCost)
    {
        this.goldCost = goldCost;
        this.woodCost = woodCost;
        this.stoneCost = stoneCost;
        this.doggiumCost = doggiumCost;
    }
}
