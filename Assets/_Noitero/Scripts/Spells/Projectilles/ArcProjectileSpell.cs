using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile/Curved Projectile")]
public class ArcProjectileSpell : SpellBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float angularSpeed = 45f;

    public override void Execute(SpellExecutionContext context)
    {
        Vector3 dir = new Vector3(context.Direction.x, 0f, context.Direction.z).normalized;
        Vector3 spawnPos = context.Caster + dir * 0.5f;
        var instance = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(dir));

        if (instance.TryGetComponent<Rigidbody>(out var rb))
        {
            var curved = instance.AddComponent<CurvedProjectile>();
            curved.Init(dir, speed, angularSpeed);
        }

        instance.AddComponent<TriggerHit>().Init(10);
        context.SpawnedProjectiles.Add(instance);

        if (context.PendingModifiers != null)
        {
            foreach (var mod in context.PendingModifiers)
                mod.Execute(context);
        }
    }
}
