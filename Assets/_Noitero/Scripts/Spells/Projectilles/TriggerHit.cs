using UnityEngine;

public class TriggerHit : MonoBehaviour
{
    private int _damage;
    public void Init(int dmg)
    {
        _damage = dmg;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IHealth>(out IHealth health))
        {
            health.Damage(_damage);
            
        }
    }
}
