using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    private List<IObjective> _objectives;

    private void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _objectives.Add(gameObject.transform.GetChild(i).gameObject.GetComponent<IObjective>());
        }
    }

    /// <summary>
    /// Checks if all primary objectives are complited
    /// </summary>
    /// <returns></returns>
    public bool CheckPrimaryObjectives()
    {
        return !_objectives.Any(o => o.IsPrimary && !o.IsComplited);
    }

    /// <summary>
    /// Checks if all objectives are complited
    /// </summary>
    /// <returns></returns>
    public bool CheckAllObjectives()
    {
        return _objectives.All(o => o.IsComplited);
    }
}
