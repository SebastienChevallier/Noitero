using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public PlayerWeaponController WeaponController;

    private void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.TryGetComponent<IHealth>(out IHealth health))
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject != this) 
            {
                Vector3 direction = Vector3.zero;
                direction =  other.transform.position - transform.position;

                WeaponController.Shoot(direction);
            }
        }
    }

    /*void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 targetPos = hit.point;
                Vector3 direction = (targetPos - transform.position);
                direction.y = 0f;
                direction.Normalize();

                
            }
        }
    }*/
}
