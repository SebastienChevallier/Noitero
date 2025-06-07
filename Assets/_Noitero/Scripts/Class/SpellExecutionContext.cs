using System.Collections.Generic;
using System;
using UnityEngine;

public class SpellExecutionContext
{
    public Vector3 Caster;
    public Vector3 Direction;
    public List<GameObject> SpawnedProjectiles = new();
    public Dictionary<string, object> Variables = new();
    public int ExecutedSpellIndex { get; set; }
    public List<SpellBase> PendingModifiers;

    public Action<int> AdvanceIndexAction { get; set; }

    public Vector3? ImpactPosition { get; set; } = null;
    public bool ImpactTriggered { get; set; } = false;

    public List<SpellBase> RemainingSpells { get; set; } = new();

    public void SetVariable(string key, object value) => Variables[key] = value;
    public T GetVariable<T>(string key) => Variables.ContainsKey(key) ? (T)Variables[key] : default;

    public List<SpellBase> NextSpellsAfter(ISpell spell)
    {
        return RemainingSpells;
        /*string spellId = spell.Id;
        int index = RemainingSpells.FindIndex(s => s.Id == spellId);

        if (index >= 0 && index + 1 < RemainingSpells.Count)
        {
            return RemainingSpells.GetRange(index + 1, RemainingSpells.Count - (index + 1));
        }

        return new List<SpellBase>();*/
    }
}
