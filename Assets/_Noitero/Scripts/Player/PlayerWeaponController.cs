using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Transform casterTransform;
    private WeaponInstance weaponInstance;
    private Camera mainCamera;

    public WeaponInstance WeaponInstance => weaponInstance;

    void Start()
    {
        weaponInstance = new WeaponInstance(weaponData, casterTransform, this);
        mainCamera = Camera.main;
    }

    public void Shoot(Vector3 direction)
    {
        weaponInstance.TryCastNext(direction);
    }
}
