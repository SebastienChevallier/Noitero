using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Modifier/Cone Next Spells")]
public class ConeNextSpellsModifier : SpellBase
{

    [SerializeField] private int shots = 3;
    [SerializeField] private float coneAngle = 30f;

    public override void Execute(SpellExecutionContext context)
    {

        List<SpellBase> remaining = context.RemainingSpells;
        int castCount = Mathf.Min(shots, remaining.Count);
        if (castCount <= 0)
            return;

        float step = (castCount > 1) ? coneAngle / (castCount - 1) : 0f;
        Vector3 baseDirection = context.Direction;
        int originIndex = context.ExecutedSpellIndex;

        List<GameObject> totalProjectiles = new();

        for (int i = 0; i < castCount; i++)
        {
            SpellBase spell = remaining[0];

            float offset = -coneAngle / 2f + step * i;
            context.Direction = Quaternion.AngleAxis(offset, Vector3.up) * baseDirection;

            context.ExecutedSpellIndex = originIndex + i + 1;
            remaining = remaining.Skip(1).ToList();
            context.RemainingSpells = remaining;
            context.SpawnedProjectiles = new List<GameObject>();

            spell.Execute(context);

            totalProjectiles.AddRange(context.SpawnedProjectiles);
            context.AdvanceIndexAction?.Invoke(context.ExecutedSpellIndex + 1);
        }

        context.SpawnedProjectiles = totalProjectiles;
        context.Direction = baseDirection;

    }
}
