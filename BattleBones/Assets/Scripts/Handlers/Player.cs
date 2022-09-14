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
    public EventHandler PlayerEventHandler;
    public List<GameObject> UnlockedUnits; // It has to be GameObject because we need Instantiate that unit
    public List<GameObject> AvailableBuildings; // It has to be GameObject because we need Instantiate that building

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
