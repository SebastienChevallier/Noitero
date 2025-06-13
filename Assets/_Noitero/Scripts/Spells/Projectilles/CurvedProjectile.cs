using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CurvedProjectile : MonoBehaviour
{
    private float _speed;
    private Vector3 _direction;
    private float _angularSpeed;
    private Rigidbody _rb;

    public void Init(Vector3 direction, float speed, float angularSpeed)
    {
        _direction = new Vector3(direction.x, 0f, direction.z).normalized;
        _speed = speed;
        _angularSpeed = angularSpeed;
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        if (_rb == null) return;
        _direction = Quaternion.AngleAxis(_angularSpeed * Time.fixedDeltaTime, Vector3.up) * _direction;
        _rb.linearVelocity = _direction * _speed;
    }
}
