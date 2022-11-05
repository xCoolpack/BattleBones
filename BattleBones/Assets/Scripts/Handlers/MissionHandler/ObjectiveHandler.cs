using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    public List<IObjective> Objectives { get; private set; }
    public List<IObjective> FailObjectives { get; private set; }

    private void Start()
    {
        Objectives = new List<IObjective>();
        FailObjectives = new List<IObjective>();
        Transform childTransform = transform.Find("Objectives").transform;
        for (int i = 0; i < childTransform.childCount; i++)
        {
            Objectives.Add(childTransform.GetChild(i).gameObject.GetComponent<IObjective>());
        }
        childTransform = transform.Find("FailObjectives").transform;
        for (int i = 0; i < childTransform.childCount; i++)
        {
            FailObjectives.Add(childTransform.GetChild(i).gameObject.GetComponent<IObjective>());
        }
    }

    /// <summary>
    /// Checks if all primary objectives are completed
    /// </summary>
    /// <returns></returns>
    public bool CheckPrimaryObjectives()
    {
        return !Objectives.Any(o => o.IsPrimary && !o.IsCompleted);
    }

    /// <summary>
    /// Checks if all objectives are completed
    /// </summary>
    /// <returns></returns>
    public bool CheckAllObjectives()
    {
        return Objectives.All(o => o.IsCompleted);
    }

    /// <summary>
    /// Checks if any fail objectives are completed
    /// </summary>
    /// <returns></returns>
    public bool CheckFailObjectives()
    {
        return FailObjectives.Any(o => o.IsCompleted);
    }
}
