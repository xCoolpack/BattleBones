using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int MaxUnitCap;
    public int CurrentUnitCap;
    public int TradeBuildingCounter;
    public string Faction;
    public ResourceManager ResourceManager;
    public MissionHandler MissionHandler;
    public PlayerTurnHandler PlayerTurnHandler;
    public List<Unit> UnlockedUnits;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
