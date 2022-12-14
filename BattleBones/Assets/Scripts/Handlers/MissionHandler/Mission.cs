using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mission : MonoBehaviour
{
    public string MissionName;
    public string MissionDescription;
    public bool IsCompleted => PlayerPrefs.GetInt(MissionName) == 1;
    public List<Mission> PreviousMissions;
    public List<ObjectiveTuple> Objectives;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(MissionName)) 
            PlayerPrefs.SetInt(MissionName, 0);
        foreach (var objective in Objectives.Where(objective => !PlayerPrefs.HasKey(objective.ObjectiveId.ToString())))
        {
            PlayerPrefs.SetInt(objective.ObjectiveId.ToString(), 0);
        }
        PlayerPrefs.Save();
    }

    private void Start()
    {
        //Objectives.ForEach(x => Debug.Log($"{x.ObjectiveId} - {PlayerPrefs.GetInt(x.ObjectiveId.ToString())}"));
    }

    public bool CanStartMission()
    {
        return PreviousMissions.All(m => m.IsCompleted);
    }

    public void StartMission()
    {
        SceneManager.LoadScene($"{MissionName}Scene");
    }

}

[System.Serializable]
public struct ObjectiveTuple
{
    /// <summary>
    /// Use format {MissionNumber}{ObjectiveNumber} -> ### e.g. 103 (first objective is 1)
    /// </summary>
    public int ObjectiveId;
    public string ObjectiveDescription;
}
