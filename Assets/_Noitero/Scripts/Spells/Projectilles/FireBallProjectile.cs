using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    private SpellExecutionContext _context;
    private List<SpellBase> _nextSpells;

    public void Initialize(SpellExecutionContext context)
    {
        // Copie du contexte et des spells suivants (shallow copy)
        _context = context;
        _nextSpells = new List<SpellBase>(context.NextSpellsAfter(this));
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 hitPos = collision.contacts[0].point;

        _context.ImpactPosition = hitPos;
        _context.ImpactTriggered = true;

        foreach (var spell in _nextSpells)
        {
            spell.Execute(_context);
        }

        Destroy(gameObject);
    }
}
