using UnityEngine;
using UnityEngine.UI;

public interface ISpell
{
    string Id { get; }
    SpellCategory Category { get; }

    Sprite Icon { get; }
    void Execute(SpellExecutionContext context);

}
