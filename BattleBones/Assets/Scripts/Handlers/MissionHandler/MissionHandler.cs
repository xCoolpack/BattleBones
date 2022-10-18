using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MissionHandler : MonoBehaviour
{
    private int _collectedTrophies => GetCollectedTrophies(); 
    private int _usedTrophies = 0; // using trophies not implemented
    public int CurrentTrophies => _collectedTrophies - _usedTrophies;
    private List<Mission> _missionList;

    private void Awake()
    {
        _missionList = new List<Mission>();
        for (int i = 0; i < transform.childCount; i++)
        {
            _missionList.Add(transform.GetChild(i).gameObject.GetComponent<Mission>());
        }

        Debug.Log(new Resources(100,100,100,100,100) >= new Resources(wood: 50));
    }

    private int GetCollectedTrophies()
    {
        return _missionList.Sum(m => m.Objectives.Sum(o => PlayerPrefs.GetInt(o.ObjectiveId.ToString())));
    }
}
