using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    public int NumOfAbilities;
    [SerializeField] [CanBeNull] private Player _player;
    [ItemCanBeNull] public List<IAbility> SelectedAbilities;
    private List<IAbility> _allAbilities;

    void Awake()
    {
        SelectedAbilities = new List<IAbility>();
        _allAbilities = new List<IAbility>();
    }

    private void Start()
    {
        _player = GetComponentInParent<Player>();

        LoadAllAbilities();
        if (_player != null)
        {
            if (_player == Player.HumanPlayer)
                LoadSelectedAbilitiesForHuman();
            else if (_player != Player.HumanPlayer)
                LoadSelectedAbilitiesForBot();
        }
    }

    private void LoadAllAbilities()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            _allAbilities.Add(transform.GetChild(i).gameObject.GetComponent<IAbility>());
        }
    }

    private void LoadSelectedAbilitiesForHuman()
    {
        for (var i = 0; i < NumOfAbilities; i++)
        {
            var id = PlayerPrefs.GetString($"SelectedAbility{i}", "-1");
            SelectedAbilities.Add(_allAbilities.FirstOrDefault(a => a.Id == id));
        }
    }

    private void LoadSelectedAbilitiesForBot()
    {
        // TO DO
    }

    public bool CanSelectAbility(string id)
    {
        return SelectedAbilities.Any(a => a?.Id == id);
    }

    public void SelectAbility(int pos, string id)
    {
        PlayerPrefs.SetString($"SelectedAbility{pos}", id);
    }

    public bool CanDeselectAbility(int pos)
    {
        return SelectedAbilities[pos] != null;
    }

    public void DeselectAbility(int pos)
    {
        PlayerPrefs.DeleteKey($"SelectedAbility{pos}");
    }

    public bool CanUseAbility(int pos, Field targetField, Player targetPlayer, Player castingPlayer)
    {
        return SelectedAbilities[pos]?.CanUse(targetField, targetPlayer, castingPlayer) ?? false;
    }

    public void UseAbility(int pos, Field targetField, Player targetPlayer, Player castingPlayer)
    {
        SelectedAbilities[pos]?.Use(targetField, targetPlayer, castingPlayer);
    }
}
