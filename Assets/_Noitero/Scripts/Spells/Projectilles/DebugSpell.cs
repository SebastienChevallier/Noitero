using UnityEngine;

[CreateAssetMenu(fileName = "DebugSpell", menuName = "Scriptable Objects/DebugSpell")]
public class DebugSpell : SpellBase
{
    public override void Execute(SpellExecutionContext context)
    {
        Debug.Log("Debug Spell");
    }
}
