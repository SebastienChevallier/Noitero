using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Modifier/ZigZag Trajectory")]
public class ZigZagModifier : SpellBase
{
    [SerializeField] private float amplitude = 2f;
    [SerializeField] private float frequency = 5f;

    public override void Execute(SpellExecutionContext context)
    {
        foreach (var proj in context.SpawnedProjectiles)
        {
            if (proj == null) continue;
            var rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                var dir = rb.linearVelocity.normalized;
                dir = new Vector3(dir.x, 0f, dir.z).normalized;
                var zigzag = proj.AddComponent<ZigZagProjectile>();
                zigzag.Init(dir, rb.linearVelocity.magnitude, amplitude, frequency);
            }
        }
    }
}
