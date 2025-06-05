using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile/Fireball")]
public class FireBallSpellTriggerOnImpact : SpellBase
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float speed = 10f;

    public override void Execute(SpellExecutionContext context)
    {
        var instance = Instantiate(fireballPrefab, context.Caster.position, Quaternion.LookRotation(context.Direction));
        var rb = instance.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = context.Direction.normalized * speed;

        context.SpawnedProjectiles.Add(instance);

        // Pr�paration du d�clenchement � l�impact
        if (TriggerNextOnImpact)
        {
            var projectile = instance.GetComponent<FireballProjectile>();
            if (projectile != null)
            {
                projectile.Initialize(context);
            }
        }
    }

}
