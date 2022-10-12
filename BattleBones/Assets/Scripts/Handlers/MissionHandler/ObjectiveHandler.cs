using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    public List<IObjective> Objectives { get; }
    private void Start()
    {
        _objectives = new List<IObjective>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Objectives.Add(gameObject.transform.GetChild(i).gameObject.GetComponent<IObjective>());
        }
    }

    /// <summary>
    /// Checks if all primary objectives are complited
    /// </summary>
    /// <returns></returns>
    public bool CheckPrimaryObjectives()
    {
        return !Objectives.Any(o => o.IsPrimary && !o.IsComplited);
    }

    /// <summary>
    /// Checks if all objectives are complited
    /// </summary>
    /// <returns></returns>
    public bool CheckAllObjectives()
    {
        return Objectives.All(o => o.IsComplited);
    }
}
