using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Context object passed to spells during execution.
/// Holds shared variables and the remaining sequence.
/// </summary>
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
}
