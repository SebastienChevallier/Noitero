using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile/Fireball")]
public class FireBallSpell : SpellBase
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float speed = 10f;

    public override void Execute(SpellExecutionContext context)
    {
        Vector3 spawnPos = context.Caster + context.Direction.normalized * 0.5f;
        var instance = GameObject.Instantiate(fireballPrefab, spawnPos, Quaternion.LookRotation(context.Direction));
        var rb = instance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = context.Direction.normalized * speed;
        }

        context.SpawnedProjectiles.Add(instance);

        if (context.PendingModifiers != null)
        {
            foreach (var mod in context.PendingModifiers)
            {
                mod.Execute(context); // chaque mod agit sur context.SpawnedProjectiles
            }
        }
    }
}
