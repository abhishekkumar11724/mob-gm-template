using System;
using System.Collections.Generic;
using Nimbus.Core;

namespace Nimbus.DebugTools;

public interface IDebugService : IService
{
    event Action<bool> VisibilityChanged;

    bool IsVisible { get; }

    void Register(IDebugEntry entry);

    void Unregister(IDebugEntry entry);

    IReadOnlyList<IDebugEntry> GetEntries();

    void SetVisible(bool visible);
}
