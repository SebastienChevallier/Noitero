using UnityEngine;

public static class SpellExecutor
{
    public static void ExecuteSpell(ISpell spell, SpellExecutionContext context)
    {
        spell.Execute(context);
    }
}
