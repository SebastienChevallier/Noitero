using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile/Arc Projectile")]
public class ArcProjectileSpell : SpellBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float upwardVelocity = 5f;

    public override void Execute(SpellExecutionContext context)
    {
        Vector3 spawnPos = context.Caster + context.Direction.normalized * 0.5f;
        var instance = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(context.Direction));

        if (instance.TryGetComponent<Rigidbody>(out var rb))
        {
            Vector3 vel = context.Direction.normalized * speed;
            vel.y += upwardVelocity;
            rb.linearVelocity = vel;
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
