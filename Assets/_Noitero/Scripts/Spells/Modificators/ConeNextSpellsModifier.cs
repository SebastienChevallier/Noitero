using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Utility/Cone Next Spells")]
public class ConeNextSpellsModifier : SpellBase
{

    [SerializeField] private int shots = 3;
    [SerializeField] private float coneAngle = 30f;

    public override void Execute(SpellExecutionContext context)
    {
        List<SpellBase> sequence = context.RemainingSpells;
        if (sequence == null || sequence.Count == 0)
            return;

        Vector3 baseDirection = context.Direction;
        int originIndex = context.ExecutedSpellIndex;

        // Modifiers coming from earlier spells (e.g. Boomerang)
        var initialMods = context.PendingModifiers != null
            ? new List<SpellBase>(context.PendingModifiers)
            : null;

        // Holds projectiles found and the modifiers preceding them
        var casts = new List<(SpellBase projectile, int index, List<SpellBase> mods, int consumed)>();
        var collectedMods = new List<SpellBase>();

        int scan = 0;
        while (scan < sequence.Count && casts.Count < shots)
        {
            SpellBase next = sequence[scan];

            if (next.Category == SpellCategory.Modifier)
            {
                collectedMods.Add(next);
                scan++;
                continue;
            }

            if (next.Category == SpellCategory.Projectile)
            {
                var mods = new List<SpellBase>();
                if (initialMods != null)
                {
                    mods.AddRange(initialMods);
                    initialMods = null; // only apply once
                }

                mods.AddRange(collectedMods);

                casts.Add((next, originIndex + scan + 1, mods, scan + 1));
                collectedMods.Clear();
                scan++;
                continue;
            }

            // Stop if a non compatible spell is encountered
            break;
        }

        if (casts.Count == 0)
            return;

        float step = (casts.Count > 1) ? coneAngle / (casts.Count - 1) : 0f;
        List<GameObject> totalProjectiles = new();

        for (int i = 0; i < casts.Count; i++)
        {
            var info = casts[i];

            float offset = -coneAngle / 2f + step * i;
            context.Direction = Quaternion.AngleAxis(offset, Vector3.up) * baseDirection;

            context.ExecutedSpellIndex = info.index;
            context.RemainingSpells = sequence.Skip(info.consumed).ToList();
            context.PendingModifiers = info.mods;
            context.SpawnedProjectiles = new List<GameObject>();

            info.projectile.Execute(context);

            totalProjectiles.AddRange(context.SpawnedProjectiles);
            context.PendingModifiers = null;
            context.AdvanceIndexAction?.Invoke(info.index + 1);
        }

        context.SpawnedProjectiles = totalProjectiles;
        context.Direction = baseDirection;
    }
}
