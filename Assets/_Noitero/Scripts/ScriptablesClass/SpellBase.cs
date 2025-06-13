using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpellBase", menuName = "Spells/SpellBase")]
public abstract class SpellBase : ScriptableObject, ISpell
{
    [SerializeField] private string id;
    [SerializeField] private SpellCategory category;
    [SerializeField] private Sprite icon;
    [SerializeField] private Color colorType;

    [SerializeField] private bool canTriggerNext = false;

    // Nouveau flag : ce sort déclenche le suivant **à l'impact**
    [SerializeField] private bool triggerNextOnImpact = false;

    public string Id => id;
    public SpellCategory Category => category;
    public bool CanTriggerNext => canTriggerNext;
    public bool TriggerNextOnImpact => triggerNextOnImpact;

    public Sprite Icon => icon;

    public Color ColorType => colorType;

    public abstract void Execute(SpellExecutionContext context);
}
