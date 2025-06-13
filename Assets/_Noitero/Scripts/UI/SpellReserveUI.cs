using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellReserveUI : MonoBehaviour, ISpellList
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject spellSlotPrefab;
    [SerializeField] private List<SpellBase> reserveSpells = new();

    private List<SpellSlotUI> slots = new();

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
    }

    private void Start()
    {
        if (content == null || spellSlotPrefab == null)
            return;

        BuildSlots(reserveSpells);
    }

    public Transform Content => content;

    public void OnItemMoved(int oldIndex, int newIndex)
    {
        if (oldIndex == newIndex)
            return;

        var item = reserveSpells[oldIndex];
        reserveSpells.RemoveAt(oldIndex);
        reserveSpells.Insert(newIndex, item);

        BuildSlots(reserveSpells);
    }

    public SpellBase RemoveSpellAt(int index)
    {
        if (index < 0 || index >= reserveSpells.Count)
            return null;
        var spell = reserveSpells[index];
        reserveSpells.RemoveAt(index);
        BuildSlots(reserveSpells);
        return spell;
    }

    public void InsertSpellAt(SpellBase spell, int index)
    {
        if (index < 0 || index > reserveSpells.Count)
            index = reserveSpells.Count;
        reserveSpells.Insert(index, spell);
        BuildSlots(reserveSpells);
    }
}
