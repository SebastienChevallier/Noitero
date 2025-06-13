using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile/Proximity Mine")]
public class ProximityMineSpell : SpellBase
{
    [SerializeField] private GameObject minePrefab;

    public override void Execute(SpellExecutionContext context)
    {
        Vector3 dir = new Vector3(context.Direction.x, 0f, context.Direction.z).normalized;
        Vector3 spawnPos = context.Caster + dir * 0.5f;

        var instance = Instantiate(minePrefab, spawnPos, Quaternion.identity);
        context.SpawnedProjectiles.Add(instance);

        if (context.PendingModifiers != null)
        {
            foreach (var mod in context.PendingModifiers)
                mod.Execute(context);
        }
    }
}
