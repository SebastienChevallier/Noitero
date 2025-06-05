using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoomerangProjectile : MonoBehaviour
{
    private Vector3 _origin;
    private float _delay;
    private float _speed;
    private bool _returning = false;
    private float _timer = 0f;

    private Rigidbody _rb;

    public void Setup(Vector3 origin, float delay, float speed)
    {
        _origin = origin;
        _delay = delay;
        _speed = speed;

        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("BoomerangProjectile requires a Rigidbody.");
        }
    }

    void FixedUpdate()
    {
        if (_rb == null) return;

        _timer += Time.fixedDeltaTime;

        if (!_returning && _timer >= _delay)
        {
            _returning = true;
        }

        if (_returning)
        {
            Vector3 direction = (_origin - transform.position).normalized;
            _rb.linearVelocity = direction * _speed;

            // Optional auto-destroy when close to origin
            if (Vector3.Distance(transform.position, _origin) < 0.3f)
            {
                Destroy(gameObject);
            }
        }
    }
}
