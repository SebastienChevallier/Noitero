using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile/Laser")]
public class LaserProjectileSpell : SpellBase
{
    [SerializeField] private GameObject laserPrefab;

    public override void Execute(SpellExecutionContext context)
    {
        Vector3 dir = new Vector3(context.Direction.x, 0f, context.Direction.z).normalized;
        Vector3 spawnPos = context.Caster;
        var instance = Instantiate(laserPrefab, spawnPos, Quaternion.LookRotation(dir));
        context.SpawnedProjectiles.Add(instance);

        if (context.PendingModifiers != null)
        {
            foreach (var mod in context.PendingModifiers)
                mod.Execute(context);
        }
    }
}
