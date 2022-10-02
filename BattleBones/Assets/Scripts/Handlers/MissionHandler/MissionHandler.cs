using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionHandler : MonoBehaviour
{
    private IMission _mission;

    private void Start()
    {
        _mission = gameObject.transform.GetChild(0).gameObject.GetComponent<IMission>();
    }

    public void CheckMission()
    {
        var result = _mission.Check();

        if (result)
        {
            Debug.Log("Mission complited!");
        }
    }
}
