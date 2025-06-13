using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Effect/Laser")]
public class LaserSpell : SpellBase
{
    [SerializeField] private float length = 10f;
    [SerializeField] private int damage = 5;

    public override void Execute(SpellExecutionContext context)
    {
        Vector3 start = context.Caster;

        Vector3 dir = new Vector3(context.Direction.x, 0f, context.Direction.z).normalized;

        if (Physics.Raycast(start, dir, out RaycastHit hit, length))
        {
            if (hit.collider.TryGetComponent<IHealth>(out var health))
                health.Damage(damage);

            context.Caster = hit.point;
        }
        else
        {
            context.Caster = start + dir * length;
        }

        context.ImpactTriggered = true;
    }
}
