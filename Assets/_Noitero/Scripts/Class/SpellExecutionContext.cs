using System.Collections.Generic;
using UnityEngine;

public class SpellExecutionContext
{
    public Transform Caster;
    public Vector3 Direction;
    public List<GameObject> SpawnedProjectiles = new();
    public Dictionary<string, object> Variables = new();

    public List<SpellBase> PendingModifiers;

    public Vector3? ImpactPosition { get; set; } = null;
    public bool ImpactTriggered { get; set; } = false;

    public List<SpellBase> RemainingSpells { get; set; } = new();

    public void SetVariable(string key, object value) => Variables[key] = value;
    public T GetVariable<T>(string key) => Variables.ContainsKey(key) ? (T)Variables[key] : default;
}
