using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nimbus.Core;

public sealed class Ticker : MonoBehaviour, ITickerService
{
    private readonly List<ITickable> _tickables = new();
    private readonly List<ILateTickable> _lateTickables = new();

    private readonly List<ITickable> _tickablesToAdd = new();
    private readonly List<ITickable> _tickablesToRemove = new();
    private readonly List<ILateTickable> _lateTickablesToAdd = new();
    private readonly List<ILateTickable> _lateTickablesToRemove = new();

    private bool _isUpdating;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Add(ITickable tickable)
    {
        if (tickable == null)
        {
            throw new ArgumentNullException(nameof(tickable));
        }

        if (_isUpdating)
        {
            _tickablesToAdd.Add(tickable);
            _tickablesToRemove.Remove(tickable);
            return;
        }

        _tickables.Add(tickable);
    }

    public void Remove(ITickable tickable)
    {
        if (tickable == null)
        {
            throw new ArgumentNullException(nameof(tickable));
        }

        if (_isUpdating)
        {
            _tickablesToRemove.Add(tickable);
            _tickablesToAdd.Remove(tickable);
            return;
        }

        _tickables.Remove(tickable);
    }

    public void Add(ILateTickable tickable)
    {
        if (tickable == null)
        {
            throw new ArgumentNullException(nameof(tickable));
        }

        if (_isUpdating)
        {
            _lateTickablesToAdd.Add(tickable);
            _lateTickablesToRemove.Remove(tickable);
            return;
        }

        _lateTickables.Add(tickable);
    }

    public void Remove(ILateTickable tickable)
    {
        if (tickable == null)
        {
            throw new ArgumentNullException(nameof(tickable));
        }

        if (_isUpdating)
        {
            _lateTickablesToRemove.Add(tickable);
            _lateTickablesToAdd.Remove(tickable);
            return;
        }

        _lateTickables.Remove(tickable);
    }

    private void Update()
    {
        _isUpdating = true;
        var deltaTime = Time.deltaTime;

        foreach (var tickable in _tickables)
        {
            tickable.Tick(deltaTime);
        }

        _isUpdating = false;
        ProcessPendingChanges();
    }

    private void LateUpdate()
    {
        _isUpdating = true;
        var deltaTime = Time.deltaTime;

        foreach (var tickable in _lateTickables)
        {
            tickable.Tick(deltaTime);
        }

        _isUpdating = false;
        ProcessPendingChanges();
    }

    private void ProcessPendingChanges()
    {
        ApplyAdditions(_tickables, _tickablesToAdd);
        ApplyRemovals(_tickables, _tickablesToRemove);
        ApplyAdditions(_lateTickables, _lateTickablesToAdd);
        ApplyRemovals(_lateTickables, _lateTickablesToRemove);
    }

    private void ApplyAdditions<T>(List<T> target, List<T> additions)
    {
        if (additions.Count == 0)
        {
            return;
        }

        target.AddRange(additions);
        additions.Clear();
    }

    private void ApplyRemovals<T>(List<T> target, List<T> removals)
    {
        if (removals.Count == 0)
        {
            return;
        }

        foreach (var item in removals)
        {
            target.Remove(item);
        }

        removals.Clear();
    }
}
