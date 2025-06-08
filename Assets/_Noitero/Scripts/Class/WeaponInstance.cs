using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponInstance
{
    private readonly WeaponData _data;
    private readonly Transform _caster;
    private MonoBehaviour _executorHost;

    private int _currentIndex = 0;
    private bool _isOnCooldown = false;
    private Coroutine _cooldownCoroutine = null;
    private List<SpellBase> _pendingModifiers = new();

    public WeaponInstance(WeaponData data, Transform caster, MonoBehaviour executorHost)
    {
        _data = data;
        _caster = caster;
        _executorHost = executorHost;
    }

    public void TryCastNext(Vector3 direction)
    {
        if (_isOnCooldown || _currentIndex >= _data.SpellSequence.Count)
            return;

        _executorHost.StartCoroutine(CastNextSpell(direction));
    }

    private IEnumerator CastNextSpell(Vector3 direction)
    {

        SpellExecutionContext context = new()
        {
            Caster = _caster.position,
            Direction = direction
        };
        context.AdvanceIndexAction = i =>
        {
            _currentIndex = i;
            if (_currentIndex >= _data.SpellSequence.Count)
                BeginCooldown();
        };


        while (_currentIndex < _data.SpellSequence.Count)
        {
            SpellBase spell = _data.SpellSequence[_currentIndex];
            _currentIndex++;

            if (spell.Category == SpellCategory.Modifier)
            {
                _pendingModifiers.Add(spell);
                continue; // ne bloque pas le clic suivant
            }

            Debug.Log($"[WeaponInstance] Executing spell at index {_currentIndex - 1} ({spell.name})");
            context.RemainingSpells = _data.SpellSequence.Skip(_currentIndex).ToList();
            context.ExecutedSpellIndex = _currentIndex - 1;
            Debug.Log($"[WeaponInstance] RemainingSpells.Count = {context.RemainingSpells.Count}");

            // Appliquer les modificateurs uniquement sur ce sort
            context.PendingModifiers = _pendingModifiers;
            spell.Execute(context);
            context.PendingModifiers = null;
            _pendingModifiers.Clear();

            // Pause entre sorts "principaux"
            break;
        }


        BeginCooldown();
        yield break;

    }
    private void BeginCooldown()
    {
        if (_cooldownCoroutine == null)
            _cooldownCoroutine = _executorHost.StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(_data.GlobalCooldown);
        _currentIndex = 0;
        _isOnCooldown = false;
        _cooldownCoroutine = null;
    }  
}
