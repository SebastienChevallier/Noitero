using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile/ZigZag Projectile")]
public class ZigZagProjectileSpell : SpellBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float amplitude = 2f;
    [SerializeField] private float frequency = 5f;

    public override void Execute(SpellExecutionContext context)
    {
        Vector3 spawnPos = context.Caster + context.Direction.normalized * 0.5f;
        var instance = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(context.Direction));

        var zigzag = instance.AddComponent<ZigZagProjectile>();
        zigzag.Init(context.Direction, speed, amplitude, frequency);

        instance.AddComponent<TriggerHit>().Init(10);
        context.SpawnedProjectiles.Add(instance);

        if (context.PendingModifiers != null)
        {
            foreach (var mod in context.PendingModifiers)
                mod.Execute(context);
        }
    }
}
