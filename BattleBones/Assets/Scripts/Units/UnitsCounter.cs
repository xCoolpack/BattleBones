using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;

public class UnitsCounter : MonoBehaviour
{
    [SerializeField] private int _counterValue;
    public List<string> Units;
    public List<List<bool>> IsCounterList;

    private void Awake()
    {
        Units = new List<string>()
        {
            //dogs
            "Dog scout",
            "Dog",
            "Dog-at-arms",
            "Archer dog",
            "Dog-a-copter", 
            "Battering dog",
            "Dog-a-pult",
            //cats
            "Cat scout",
            "Cat",
            "Cat-at-arms",
            "Crossbowcat",
            "Cat-a-copter",
            "Mountain cat",
        };

        IsCounterList = new List<List<bool>>()
        {
            //Dog scout
            new List<bool>()
            {
                false, false, false, false, false, false, false, false, false, false, false, false, false
            },
            //Dog
            new List<bool>()
            {
                false, false, false, false, false, false, true, false, false, false, false, false, false
            },
            //Dog-at-arms
            new List<bool>()
            {
                false, false, false, false, false, true, true, false, false, false, false, false, false
            },
            //Crossbowdog
            new List<bool>()
            {
                true, true, false, false, false, true, true, true, true, false, false, false, false
            },
            //Dog-a-copter
            new List<bool>()
            {
                false, false, false, true, false, false, true, false, false, false, true, false, false
            },
            //Battering dog
            new List<bool>()
            {
                false, false, false, false, false, false, false, false, false, false, false, false, false
            },
            //Dog-a-pult
            new List<bool>()
            {
                false, false, false, false, false, false, false, false, false, false, false, false, false
            },
            //Cat scout
            new List<bool>()
            {
                false, false, false, false, false, false, false, false, false, false, false, false, false
            },
            //Cat
            new List<bool>()
            {
                false, false, false, false, false, false, true, false, false, false, false, false, false
            },
            //Cat-at-arms
            new List<bool>()
            {
                false, false, false, false, false, true, true, false, false, false, false, false, false
            },
            //Crossbowcat
            new List<bool>()
            {
                true, true, false, false, false, true, true, true, true, false, false, false, false
            },
            //Cat-a-copter
            new List<bool>()
            {
                false, false, false, true, false, false, true, false, false, false, true, false, false
            },
            //Mountain cat
            new List<bool>()
            {
                false, false, false, false, false, true, true, false, false, false, false, false, false
            },
        };
    }

    public bool IsUnitCounteredBy(string defender, string attacker)
    {
        return IsCounterList[Units.IndexOf(attacker)][Units.IndexOf(defender)];
    }

    public UnitModifiers GetCounterModifier(string defender, string attacker, UnitModifiers unitModifiers)
    {
        return unitModifiers + (IsUnitCounteredBy(defender, attacker)
            ? new UnitModifiers(damage: _counterValue)
            : new UnitModifiers());
    }
}
