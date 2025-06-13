using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Modifier/Speed Multiplier")]
public class SpeedMultiplierModifier : SpellBase
{
    [SerializeField] private float multiplier = 1.5f;

    public override void Execute(SpellExecutionContext context)
    {
        foreach (var proj in context.SpawnedProjectiles)
        {
            if (proj == null) continue;
            var rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity *= multiplier;
            }
        }
    }
}
