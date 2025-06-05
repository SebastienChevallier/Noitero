using UnityEngine;

public interface ISpell
{
    string Id { get; }
    SpellCategory Category { get; }
    void Execute(SpellExecutionContext context);

}
