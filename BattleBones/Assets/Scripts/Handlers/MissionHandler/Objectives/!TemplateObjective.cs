using System;
using UnityEngine;

public class TemplateObjective : MonoBehaviour, IObjective
{
    [field: SerializeField]
    public bool IsPrimary { get; set; }

    public bool IsComplited => CompletionCheck();

    [field: SerializeField]
    public string ObjectiveInfo { get; set; }

    private bool CompletionCheck() 
    {
        throw new NotImplementedException();
    }
}
