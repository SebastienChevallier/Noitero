using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance
{
    private readonly WeaponData _data;
    private readonly Transform _caster;
    private MonoBehaviour _executorHost;

    private int _currentIndex = 0;
    private bool _isOnCooldown = false;
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
            Caster = _caster,
            Direction = direction
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

            // Appliquer les modificateurs uniquement sur ce sort
            context.PendingModifiers = _pendingModifiers;
            spell.Execute(context);
            context.PendingModifiers = null;
            _pendingModifiers.Clear();

            // Pause entre sorts "principaux"
            break;
        }

        // Fin de la séquence ?
        if (_currentIndex >= _data.SpellSequence.Count)
        {
            _isOnCooldown = true;
            yield return new WaitForSeconds(_data.GlobalCooldown);
            _currentIndex = 0;
            _isOnCooldown = false;
        }
    }
}
