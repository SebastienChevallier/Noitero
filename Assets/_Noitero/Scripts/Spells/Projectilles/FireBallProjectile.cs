using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    private SpellExecutionContext _context;
    private List<SpellBase> _nextSpells;

    private int _originIndex;

    public void Initialize(SpellExecutionContext context, int originIndex)
    {
        _context = context;
        _originIndex = originIndex;
        _nextSpells = context.RemainingSpells.ToList();

        Debug.Log($"[Projectile] Origin index: {originIndex}");
        Debug.Log($"[Projectile] RemainingSpells.Count = {context.RemainingSpells.Count}");
        Debug.Log($"[Projectile] NextSpells.Count = {_nextSpells.Count}");
    }

    private void OnCollisionEnter(Collision collision)
    {        
        Vector3 hitPos = collision.contacts[0].point;

        _context.Caster = hitPos;
        _context.ImpactTriggered = true;
        
        for (int i = 0; i < _nextSpells.Count; i++)
        {
            SpellBase spell = _nextSpells[i];

            // Update context so chained spells don't re-execute previous ones
            _context.ExecutedSpellIndex = _originIndex + i + 1;
            _context.RemainingSpells = _nextSpells.Skip(i + 1).ToList();

            // keep weapon sequence in sync
            _context.AdvanceIndexAction?.Invoke(_context.ExecutedSpellIndex + 1);


            Debug.Log($"[Projectile] Executing spell: {spell.name} at index {_context.ExecutedSpellIndex}");

            spell.Execute(_context);

            if (spell.TriggerNextOnImpact)
                break; // next spells will be executed after impact of this one
        }

        
        Destroy(gameObject);
    }    
}
