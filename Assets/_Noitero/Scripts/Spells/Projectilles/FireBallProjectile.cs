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
        _nextSpells = context.RemainingSpells.Skip(originIndex).ToList();

        Debug.Log($"[Projectile] Origin index: {originIndex}");
        Debug.Log($"[Projectile] RemainingSpells.Count = {context.RemainingSpells.Count}");
        Debug.Log($"[Projectile] NextSpells.Count = {_nextSpells.Count}");
    }

    private void OnCollisionEnter(Collision collision)
    {        
        Vector3 hitPos = collision.contacts[0].point;

        _context.Caster = hitPos;
        _context.ImpactTriggered = true;
        
        foreach (var spell in _nextSpells)
        {
            spell.Execute(_context);
        }

        
        Destroy(gameObject);
    }    
}
