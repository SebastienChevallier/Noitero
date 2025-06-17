using UnityEngine;

public class WeaponToolsUI : MonoBehaviour
{
    public WeaponData weaponData;

    public void SetWeaponSpellCooldown(float cooldown)
    {
        if (weaponData != null)
        {
            weaponData.delayBetweenSpells = cooldown;
        }
    }

    public void SetWeaponGlobalCooldown(float cooldown)
    {
        if (weaponData != null)
        {
            weaponData.globalCooldown = cooldown;
        }
    }
}
