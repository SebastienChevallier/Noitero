using UnityEngine;

[CreateAssetMenu(fileName = "SpellBase", menuName = "Spells/SpellBase")]
public abstract class SpellBase : ScriptableObject, ISpell
{
    [SerializeField] private string id;
    [SerializeField] private SpellCategory category;
    [SerializeField] private bool canTriggerNext = false;

    // Nouveau flag : ce sort d�clenche le suivant **� l'impact**
    [SerializeField] private bool triggerNextOnImpact = false;

    public string Id => id;
    public SpellCategory Category => category;
    public bool CanTriggerNext => canTriggerNext;
    public bool TriggerNextOnImpact => triggerNextOnImpact;

    public abstract void Execute(SpellExecutionContext context);
}
