using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MissionHandler : MonoBehaviour
{
    private int _collectedTrophies => GetCollectedTrophies(); 
    private int _usedTrophies = 0; // using trophies not implemented
    public int CurrentTrophies => _collectedTrophies - _usedTrophies;
    public List<Mission> MissionList;

    private void Awake()
    {
        MissionList = new List<Mission>();
        for (int i = 0; i < transform.childCount; i++)
        {
            MissionList.Add(transform.GetChild(i).gameObject.GetComponent<Mission>());
        }
    }

    private int GetCollectedTrophies()
    {
        return MissionList.Sum(m => m.Objectives.Sum(o => PlayerPrefs.GetInt(o.ObjectiveId.ToString())));
    }
}
