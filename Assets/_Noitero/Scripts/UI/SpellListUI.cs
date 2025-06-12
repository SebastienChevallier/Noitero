using System.Collections.Generic;
using UnityEngine;

public class SpellListUI : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController weaponController;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject spellSlotPrefab;

    private List<SpellSlotUI> slots = new();

    private void Start()
    {
        if (weaponController == null || content == null || spellSlotPrefab == null)
            return;

        var instance = weaponController.WeaponInstance;
        if (instance == null)
            return;

        for (int i = 0; i < instance.SpellSequence.Count; i++)
        {
            var slotGO = Instantiate(spellSlotPrefab, content);
            var slot = slotGO.GetComponent<SpellSlotUI>();
            slot.Setup(this, instance.SpellSequence[i], i);
            slots.Add(slot);
        }
    }

    public void OnItemMoved(int oldIndex, int newIndex)
    {
        if (oldIndex == newIndex)
            return;

        var instance = weaponController.WeaponInstance;
        instance.MoveSpell(oldIndex, newIndex);

        var slot = slots[oldIndex];
        slots.RemoveAt(oldIndex);
        slots.Insert(newIndex, slot);

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].UpdateIndex(i);
        }
    }
}
