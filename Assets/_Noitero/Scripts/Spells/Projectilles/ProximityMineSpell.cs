using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile/Proximity Mine")]
public class ProximityMineSpell : SpellBase
{
    [SerializeField] private GameObject minePrefab;

    public override void Execute(SpellExecutionContext context)
    {
        Vector3 spawnPos = context.Caster + context.Direction.normalized * 0.5f;
        var instance = Instantiate(minePrefab, spawnPos, Quaternion.identity);
        context.SpawnedProjectiles.Add(instance);

        if (context.PendingModifiers != null)
        {
            foreach (var mod in context.PendingModifiers)
                mod.Execute(context);
        }
    }
}
