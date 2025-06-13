using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public PlayerWeaponController WeaponController;
    private PlayerMovement _movement;

    private void Start() => _movement = GetComponent<PlayerMovement>();

    private void OnTriggerStay(Collider other)
    {
        if (_movement != null && _movement.GetJoystickDirection().magnitude > 0)
            return;

        if (other.TryGetComponent<IHealth>(out IHealth _)
            && other.gameObject != gameObject)
        {
            Vector3 direction = other.transform.position - transform.position;
            WeaponController.Shoot(direction);
        }
    }

    
}
