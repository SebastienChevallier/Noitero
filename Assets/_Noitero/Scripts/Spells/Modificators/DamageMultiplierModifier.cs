using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Modifier/Damage Multiplier")]
public class DamageMultiplierModifier : SpellBase
{
    [SerializeField] private float multiplier = 1.5f;

    public override void Execute(SpellExecutionContext context)
    {
        foreach (var proj in context.SpawnedProjectiles)
        {
            if (proj == null) continue;
            foreach (var hit in proj.GetComponentsInChildren<TriggerHit>())
            {
                hit.MultiplyDamage(multiplier);
            }
        }
    }
}
