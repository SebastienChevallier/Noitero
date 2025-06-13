using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponInstance
{
    private readonly WeaponData _data;
    private readonly Transform _caster;
    private MonoBehaviour _executorHost;
    // Ordered list exposed to the UI so players can reorder spells
    private List<SpellBase> _spellSequence;

    // Queue used at runtime to execute spells
    private Queue<SpellBase> _spellQueue;

    // Snapshot of the data asset sequence to detect runtime changes
    private List<SpellBase> _dataSnapshot;

    private int _currentIndex = 0;       // index of the spell being executed
    private bool _isOnCooldown = false;  // global cooldown flag
    private bool _spellOnCooldown = false; // delay between individual casts
    private Coroutine _cooldownCoroutine;
    private Coroutine _spellCooldownCoroutine;

    // Modifiers collected before the next non-modifier spell is cast
    private List<SpellBase> _pendingModifiers = new();

    /// <summary>
    /// Ensures the runtime sequence matches the data asset. If the asset
    /// has been modified (order or count) while playing, rebuild the queue
    /// so newly added or reordered spells are taken into account.
    /// </summary>
    private void EnsureSequenceUpToDate()
    {
        var dataSeq = _data.SpellSequence;
        if (dataSeq.Count != _dataSnapshot.Count)
        {
            _spellSequence = new List<SpellBase>(dataSeq);
            _dataSnapshot = new List<SpellBase>(dataSeq);
            ResetSpellQueue();
            return;
        }

        for (int i = 0; i < dataSeq.Count; i++)
        {
            if (dataSeq[i] != _dataSnapshot[i])
            {
                _spellSequence = new List<SpellBase>(dataSeq);
                _dataSnapshot = new List<SpellBase>(dataSeq);
                ResetSpellQueue();
                break;
            }
        }
    }

    public WeaponInstance(WeaponData data, Transform caster, MonoBehaviour executorHost)
    {
        _data = data;
        _caster = caster;
        _executorHost = executorHost;
        _spellSequence = new List<SpellBase>(_data.SpellSequence);
        _dataSnapshot = new List<SpellBase>(_data.SpellSequence);

        ResetSpellQueue();
    }

    public List<SpellBase> SpellSequence => _spellSequence;

    /// <summary>
    /// Rebuilds the runtime queue from the current spell sequence.
    /// Also resets the current execution index so the sequence
    /// starts from the beginning.
    /// </summary>
    private void ResetSpellQueue()
    {
        _spellQueue = new Queue<SpellBase>(_spellSequence);
        _currentIndex = 0;
    }

    public void MoveSpell(int oldIndex, int newIndex)
    {
        if (oldIndex < 0 || oldIndex >= _spellSequence.Count ||
            newIndex < 0 || newIndex >= _spellSequence.Count)
            return;

        SpellBase item = _spellSequence[oldIndex];
        _spellSequence.RemoveAt(oldIndex);
        _spellSequence.Insert(newIndex, item);

        // Keep the underlying data asset in sync
        _data.MoveSpell(oldIndex, newIndex);

        // Rebuild queue so the runtime order matches the UI order
        ResetSpellQueue();

        // Update snapshot so EnsureSequenceUpToDate doesn't revert the change
        _dataSnapshot = new List<SpellBase>(_spellSequence);
    }

    /// <summary>
    /// Attempts to cast the next spell in the queue.
    /// If the queue is empty it will be rebuilt from the sequence.
    /// </summary>
    public void TryCastNext(Vector3 direction)
    {
        if (_isOnCooldown || _spellOnCooldown)
            return;

        EnsureSequenceUpToDate();

        if (_spellQueue == null || _spellQueue.Count == 0)
            ResetSpellQueue();

        _executorHost.StartCoroutine(CastNextSpell(direction));
    }

    /// <summary>
    /// Handles the actual execution of a spell with all modifiers.
    /// Uses a coroutine so delays between spells can be easily added later.
    /// </summary>
    private IEnumerator CastNextSpell(Vector3 direction)
    {
        SpellExecutionContext context = new()
        {
            Caster = _caster.position,
            Direction = direction
        };
        context.AdvanceIndexAction = i =>
        {
            int steps = i - _currentIndex;
            for (int s = 0; s < steps && _spellQueue.Count > 0; s++)
                _spellQueue.Dequeue();

            _currentIndex = i;

            if (_currentIndex >= _spellSequence.Count)
                BeginCooldown();
        };


        while (_spellQueue.Count > 0)
        {
            // Dequeue the next spell to process
            SpellBase spell = _spellQueue.Dequeue();
            _currentIndex++;

            if (spell.Category == SpellCategory.Modifier)
            {
                _pendingModifiers.Add(spell);
                continue; // ne bloque pas le clic suivant
            }

            // Remaining spells are those left in the queue
            context.RemainingSpells = _spellQueue.ToList();
            context.ExecutedSpellIndex = _currentIndex - 1;

            // Apply collected modifiers only to this spell
            context.PendingModifiers = _pendingModifiers;
            spell.Execute(context);
            context.PendingModifiers = null;
            _pendingModifiers.Clear();

            // Only one main spell per click
            BeginSpellCooldown();
            break;
        }

        // When no more spells remain, start the global cooldown
        if (_spellQueue.Count == 0)
            BeginCooldown();

        yield break;

    }
    /// <summary>
    /// Launches the global cooldown coroutine if not already running.
    /// </summary>
    private void BeginCooldown()
    {
        if (_cooldownCoroutine == null)
            _cooldownCoroutine = _executorHost.StartCoroutine(CooldownRoutine());
    }

    /// <summary>
    /// Waits for the global cooldown then resets the spell queue.
    /// </summary>
    private IEnumerator CooldownRoutine()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(_data.GlobalCooldown);
        ResetSpellQueue();
        _isOnCooldown = false;
        _cooldownCoroutine = null;
    }

    /// <summary>
    /// Handles the delay between individual spell casts.
    /// </summary>
    private void BeginSpellCooldown()
    {
        if (_spellCooldownCoroutine == null)
            _spellCooldownCoroutine = _executorHost.StartCoroutine(SpellCooldownRoutine());
    }

    private IEnumerator SpellCooldownRoutine()
    {
        _spellOnCooldown = true;
        yield return new WaitForSeconds(_data.DelayBetweenSpells);
        _spellOnCooldown = false;
        _spellCooldownCoroutine = null;
    }
}
