using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IObjective
{
    /// <summary>
    /// Is objective primary
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Objective id
    /// </summary>
    public int ObjectiveId { get; set; }

    /// <summary>
    /// Is objective completed
    /// </summary>
    /// <returns></returns>
    public bool IsCompleted { get; }

    /// <summary>
    /// Objective information
    /// </summary>
    public string ObjectiveInfo { get; }
}
