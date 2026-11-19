using System;
using System.Collections.Generic;
using Nimbus.Core;

namespace Nimbus.DebugTools;

public sealed class DebugService : IDebugService
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD || NIMBUS_ENABLE_DEBUG_TOOLS
    private readonly List<IDebugEntry> _entries = new();

    public event Action<bool>? VisibilityChanged;

    public bool IsVisible { get; private set; }

    public void Register(IDebugEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentNullException(nameof(entry));
        }

        if (!_entries.Contains(entry))
        {
            _entries.Add(entry);
        }
    }

    public void Unregister(IDebugEntry entry)
    {
        _entries.Remove(entry);
    }

    public IReadOnlyList<IDebugEntry> GetEntries()
    {
        return _entries;
    }

    public void SetVisible(bool visible)
    {
        if (IsVisible == visible)
        {
            return;
        }

        IsVisible = visible;
        VisibilityChanged?.Invoke(visible);
    }
#else
    public event Action<bool>? VisibilityChanged { add { } remove { } }

    public bool IsVisible => false;

    public void Register(IDebugEntry entry)
    {
    }

    public void Unregister(IDebugEntry entry)
    {
    }

    public IReadOnlyList<IDebugEntry> GetEntries()
    {
        return Array.Empty<IDebugEntry>();
    }

    public void SetVisible(bool visible)
    {
    }
#endif
}
