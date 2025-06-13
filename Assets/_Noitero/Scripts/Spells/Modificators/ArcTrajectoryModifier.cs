using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Modifier/Arc Trajectory")]
public class ArcTrajectoryModifier : SpellBase
{
    [SerializeField] private float angularSpeed = 45f;

    public override void Execute(SpellExecutionContext context)
    {
        foreach (var proj in context.SpawnedProjectiles)
        {
            if (proj == null) continue;
            var rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                var curved = proj.AddComponent<CurvedProjectile>();
                curved.Init(rb.linearVelocity.normalized, rb.linearVelocity.magnitude, angularSpeed);
            }
        }
    }
}
