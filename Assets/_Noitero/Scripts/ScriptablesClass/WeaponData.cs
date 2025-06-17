using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private string weaponId;
    [SerializeField] private List<SpellBase> spellSequence;
    [SerializeField] public float delayBetweenSpells = 0.1f;
    [SerializeField] public float globalCooldown = 1.0f;

    public float GlobalCooldown => globalCooldown;
    public string WeaponId => weaponId;
    public IReadOnlyList<SpellBase> SpellSequence => spellSequence;
    public float DelayBetweenSpells => delayBetweenSpells;

    /// <summary>
    /// Reorders a spell inside the serialized sequence.
    /// This allows runtime UIs to alter the base list.
    /// </summary>
    public void MoveSpell(int oldIndex, int newIndex)
    {
        if (oldIndex < 0 || oldIndex >= spellSequence.Count ||
            newIndex < 0 || newIndex >= spellSequence.Count)
            return;

        SpellBase item = spellSequence[oldIndex];
        spellSequence.RemoveAt(oldIndex);
        spellSequence.Insert(newIndex, item);
    }

    public void RemoveSpellAt(int index)
    {
        if (index < 0 || index >= spellSequence.Count)
            return;
        spellSequence.RemoveAt(index);
    }

    public void InsertSpell(int index, SpellBase spell)
    {
        if (index < 0 || index > spellSequence.Count)
            index = spellSequence.Count;
        spellSequence.Insert(index, spell);
    }
}
