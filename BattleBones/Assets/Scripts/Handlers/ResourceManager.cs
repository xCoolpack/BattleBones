using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int GoldAmount;
    public int WoodAmount;
    public int StoneAmount;
    public int DoggiumAmount;
    public int BoneAmount;

    public int GoldIncome;
    public int WoodIncome;
    public int StoneIncome;
    public int DoggiumIncome;
    public int BoneIncome;

    public void GenerateIncome()
    {
        GoldAmount += GoldIncome;
        WoodAmount += WoodIncome;
        StoneAmount += StoneIncome;
        DoggiumAmount += DoggiumIncome;
        BoneAmount += BoneIncome;
    }
}
