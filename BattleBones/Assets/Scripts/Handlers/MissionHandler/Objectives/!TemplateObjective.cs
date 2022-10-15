using System;
using UnityEngine;

public class TemplateObjective : MonoBehaviour, IObjective
{
    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    [field: SerializeField]
    public string ObjectiveInfo { get; set; }

    private bool CompletionCheck() 
    {
        throw new NotImplementedException();
    }
}
