using UnityEngine;

public class ProximityMine : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IHealth>(out _))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var h in hits)
        {
            if (h.TryGetComponent<IHealth>(out var health))
            {
                health.Damage(damage);
            }
        }
        Destroy(gameObject);
    }
}
