using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile/Fireball")]
public class FireBallSpellTriggerOnImpact : SpellBase
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float speed = 10f;

    public override void Execute(SpellExecutionContext context)
    {
        
        Vector3 spawnPos = context.Caster + context.Direction.normalized * 0.5f;
        var instance = Instantiate(fireballPrefab, spawnPos, Quaternion.LookRotation(context.Direction));
        var rb = instance.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = context.Direction.normalized * speed;

        context.SpawnedProjectiles.Add(instance);

        if (context.PendingModifiers != null)
        {
            foreach (var mod in context.PendingModifiers)
            {
                mod.Execute(context);
            }
        }

        instance.AddComponent<TriggerHit>().Init(10);

        // Pr�paration du d�clenchement � l�impact
        if (TriggerNextOnImpact)
        {
            
            if (instance.TryGetComponent<FireBallProjectile>(out FireBallProjectile projectile)) 
            {
                //Debug.Log(projectile.name);
                if (projectile != null)
                {
                    projectile.Initialize(context, context.ExecutedSpellIndex);
                }
            }
            

        }
    }

}
