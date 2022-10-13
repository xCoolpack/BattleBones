using System.Linq;
using UnityEngine;

public class Mission1FailObjective : MonoBehaviour, IObjective
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
        return _player.Buildings.FirstOrDefault(building => building.gameObject.GetComponent<Outpost>() != null) == null;
    }
}
