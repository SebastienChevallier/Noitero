using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private string weaponId;
    [SerializeField] private List<SpellBase> spellSequence;
    [SerializeField] private float delayBetweenSpells = 0.1f;
    [SerializeField] private float globalCooldown = 1.0f;

    public float GlobalCooldown => globalCooldown;
    public string WeaponId => weaponId;
    public IReadOnlyList<SpellBase> SpellSequence => spellSequence;
    public float DelayBetweenSpells => delayBetweenSpells;
}
