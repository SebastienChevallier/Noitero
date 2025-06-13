using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [SerializeField] private float length = 10f;
    [SerializeField] private int damage = 5;

    private void Start()
    {
        Vector3 origin = transform.position;
        Vector3 dir = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, length))
        {
            if (hit.collider.TryGetComponent<IHealth>(out var health))
                health.Damage(damage);
            transform.position = hit.point;
        }
        else
        {
            transform.position = origin + dir * length;
        }

        Destroy(gameObject, 0.1f);
    }
}
