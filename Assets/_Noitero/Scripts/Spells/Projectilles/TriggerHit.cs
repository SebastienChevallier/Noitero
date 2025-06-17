using UnityEngine;

public class TriggerHit : MonoBehaviour
{
    private int _damage;
    private bool _destroyOnHit = true;  

    public void Init(int dmg, bool destroy = false)
    {
        _damage = dmg;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IHealth>(out IHealth health))
        {
            health.Damage(_damage);
            if(_destroyOnHit)
            {
                Destroy(gameObject);
            }   
        }
    }
}
