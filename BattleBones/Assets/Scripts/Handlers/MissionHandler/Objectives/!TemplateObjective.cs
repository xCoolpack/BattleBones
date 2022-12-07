using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateObjective : MonoBehaviour, IObjective
{
    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => "Info";

    private bool CompletionCheck() 
    {
        throw new NotImplementedException();
    }
}
