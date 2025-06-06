using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Modifier/Boomerang")]
public class BoomerangModifier : SpellBase
{
    [SerializeField] private float returnDelay = 0.5f;
    [SerializeField] private float returnSpeed = 12f;

    public override void Execute(SpellExecutionContext context)
    {
        foreach (var proj in context.SpawnedProjectiles)
        {
            if (proj == null) continue;

            var boomerang = proj.AddComponent<BoomerangProjectile>();
            boomerang.Setup(context.Caster, returnDelay, returnSpeed);
        }
    }
}
