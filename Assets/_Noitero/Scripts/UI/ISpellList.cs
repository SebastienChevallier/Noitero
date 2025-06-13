using UnityEngine;

public interface ISpellList
{
    Transform Content { get; }
    void OnItemMoved(int oldIndex, int newIndex);
    SpellBase RemoveSpellAt(int index);
    void InsertSpellAt(SpellBase spell, int index);
}
