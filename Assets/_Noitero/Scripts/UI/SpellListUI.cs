using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellListUI : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController weaponController;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject spellSlotPrefab;

    private List<SpellSlotUI> slots = new();
    private List<SpellBase> sequenceSnapshot;

    private void BuildSlots(IReadOnlyList<SpellBase> sequence)
    {
        foreach (var slot in slots)
            Destroy(slot.gameObject);
        slots.Clear();

        for (int i = 0; i < sequence.Count; i++)
        {
            var slotGO = Instantiate(spellSlotPrefab, content);
            var slot = slotGO.GetComponent<SpellSlotUI>();
            slot.Setup(this, sequence[i], i);
            slots.Add(slot);
        }

        sequenceSnapshot = sequence.ToList();
    }

    private void Start()
    {
        if (weaponController == null || content == null || spellSlotPrefab == null)
            return;

        var instance = weaponController.WeaponInstance;
        if (instance == null)
            return;

        BuildSlots(instance.SpellSequence);
    }

    private void Update()
    {
        var instance = weaponController.WeaponInstance;
        if (instance == null)
            return;

        var sequence = instance.SpellSequence;
        if (sequenceSnapshot == null || sequenceSnapshot.Count != sequence.Count ||
            !sequenceSnapshot.SequenceEqual(sequence))
        {
            BuildSlots(sequence);
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

        sequenceSnapshot = instance.SpellSequence.ToList();
    }
}
