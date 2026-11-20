using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;
using UnityEngine;

namespace Nimbus.Input;

/// <summary>
/// Concrete implementation of the IInputService.
/// Manages a stack of input contexts to control which actions are active.
/// </summary>
public sealed class InputService : IInputService, IInitializable
{
    private readonly Dictionary<string, InputContext> _contexts = new();
    private readonly Stack<string> _contextStack = new();

    /// <inheritdoc />
    public event Action<Vector2>? OnTap;

    /// <inheritdoc />
    public event Action<Vector2>? OnSwipe;

    /// <inheritdoc />
    public event Action<float>? OnPinch;

    /// <inheritdoc />
    public event Action<Vector2>? OnLongPress;

    /// <inheritdoc />
    public event Action<Vector2>? OnDrag;

    /// <summary>
    /// Initializes a new instance of the <see cref="InputService"/> class.
    /// </summary>
    /// <param name="bindings">Collection of context-to-action-map bindings.</param>
    public InputService(IEnumerable<InputActionContextBinding> bindings)
    {
        if (bindings == null)
        {
            throw new ArgumentNullException(nameof(bindings));
        }

        foreach (var binding in bindings)
        {
            if (string.IsNullOrEmpty(binding.ContextName))
                continue;

            if (!_contexts.TryGetValue(binding.ContextName, out var context))
            {
                context = new InputContext(binding.ContextName);
                _contexts.Add(binding.ContextName, context);
            }

            foreach (var mapName in binding.ActionMapNames)
            {
                var map = binding.Asset.FindActionMap(mapName);
                if (map != null)
                {
                    context.AddActionMap(map);
                }
                else
                {
                    Log.Input.Warning($"Action map '{mapName}' not found in asset '{binding.Asset.name}' for context '{binding.ContextName}'.");
                }
            }
        }
    }

    /// <summary>
    /// Default constructor for common cases.
    /// </summary>
    public InputService() : this(Array.Empty<InputActionContextBinding>())
    {
    }

    /// <inheritdoc />
    public UniTask InitializeAsync(CancellationToken cancellationToken)
    {
        Log.Input.Info("InputService initialized.");
        return UniTask.CompletedTask;
    }

    /// <inheritdoc />
    public void PushContext(string contextName)
    {
        if (string.IsNullOrEmpty(contextName))
            return;

        if (!_contexts.ContainsKey(contextName))
        {
            Log.Input.Warning($"Cannot push input context '{contextName}': context not defined in bindings.");
            return;
        }

        if (_contextStack.Count > 0)
        {
            var currentContextName = _contextStack.Peek();
            _contexts[currentContextName].Disable();
        }

        _contextStack.Push(contextName);
        _contexts[contextName].Enable();
        Log.Input.Info($"Pushed input context: {contextName}");
    }

    /// <inheritdoc />
    public void PopContext()
    {
        if (_contextStack.Count == 0)
        {
            Log.Input.Warning("Cannot pop input context: stack is empty.");
            return;
        }

        var poppedContextName = _contextStack.Pop();
        _contexts[poppedContextName].Disable();

        if (_contextStack.Count > 0)
        {
            var nextContextName = _contextStack.Peek();
            _contexts[nextContextName].Enable();
            Log.Input.Info($"Popped input context: {poppedContextName}. New active context: {nextContextName}");
        }
        else
        {
            Log.Input.Info($"Popped input context: {poppedContextName}. Stack is now empty.");
        }
    }

    // Gesture triggers (to be called by internal recognizers in INPUT-06)
    internal void TriggerTap(Vector2 position) => OnTap?.Invoke(position);
    internal void TriggerSwipe(Vector2 direction) => OnSwipe?.Invoke(direction);
    internal void TriggerPinch(float delta) => OnPinch?.Invoke(delta);
    internal void TriggerLongPress(Vector2 position) => OnLongPress?.Invoke(position);
    internal void TriggerDrag(Vector2 delta) => OnDrag?.Invoke(delta);
}
