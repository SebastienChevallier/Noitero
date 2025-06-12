using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public PlayerWeaponController WeaponController;
    private PlayerMovement PlayerMovement;

    private void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (PlayerMovement != null)
        {            
            if (PlayerMovement.GetJoystickDirection().magnitude > 0)
                return;
        }

        if(other.gameObject.TryGetComponent<IHealth>(out IHealth health))
        {
            
            if (other.gameObject != this) 
            {
                Vector3 direction = Vector3.zero;
                direction =  other.transform.position - transform.position;

                WeaponController.Shoot(direction);
            }
        }
    }

    
}
