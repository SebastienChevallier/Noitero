using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Transform casterTransform;
    private WeaponInstance weaponInstance;
    private Camera mainCamera;

    void Start()
    {
        weaponInstance = new WeaponInstance(weaponData, casterTransform, this);
        mainCamera = Camera.main;
    }

    void Update()
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

                weaponInstance.TryCastNext(direction);
            }
        }
    }
}
