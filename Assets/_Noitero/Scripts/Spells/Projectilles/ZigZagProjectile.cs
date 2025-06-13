using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ZigZagProjectile : MonoBehaviour
{
    private float _speed;
    private Vector3 _direction;
    private float _amplitude;
    private float _frequency;
    private Rigidbody _rb;
    private Vector3 _right;
    private float _time;

    public void Init(Vector3 direction, float speed, float amplitude, float frequency)
    {
        _direction = direction.normalized;
        _speed = speed;
        _amplitude = amplitude;
        _frequency = frequency;
        _right = Vector3.Cross(Vector3.up, _direction).normalized;
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        if (_rb == null) return;
        _time += Time.fixedDeltaTime;
        float offset = Mathf.Sin(_time * _frequency) * _amplitude;
        Vector3 velocity = _direction * _speed + _right * offset;
        _rb.linearVelocity = velocity;
    }
}
