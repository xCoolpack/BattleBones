using System.Linq;
using UnityEngine;

public class Mission1SideObjective2 : MonoBehaviour, IObjective
{
    [SerializeField]
    private Player _player;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    public bool IsComplited => CompletionCheck();

    [field: SerializeField]
    public string ObjectiveInfo { get; set; }

    private bool CompletionCheck() 
    {
        return _player.Units.Count >= 10;
    }
}
