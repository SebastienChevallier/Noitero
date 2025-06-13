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
        int castCount = Mathf.Min(1, shots);
        if (castCount <= 0)
            return;

        float step = (castCount > 1) ? coneAngle / (castCount - 1) : 0f;
        Vector3 baseDir = context.Direction;
        int originIndex = context.ExecutedSpellIndex;

        List<GameObject> totalProjectiles = new();

        for (int i = 0; i < castCount; i++)
        {
            SpellBase spell = context.RemainingSpells[0];

            float offset = -coneAngle / 2f + step * i;
            context.Direction = Quaternion.AngleAxis(offset, Vector3.up) * baseDir;
            context.ExecutedSpellIndex = originIndex + i + 1;
            context.RemainingSpells = context.RemainingSpells.Skip(1).ToList();
            context.SpawnedProjectiles = new List<GameObject>();

            spell.Execute(context);

            totalProjectiles.AddRange(context.SpawnedProjectiles);
            context.AdvanceIndexAction?.Invoke(context.ExecutedSpellIndex + 1);
        }

        context.SpawnedProjectiles = totalProjectiles;
    }
}
