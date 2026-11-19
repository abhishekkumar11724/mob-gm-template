using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Nimbus.Input;

public sealed class InputContext
{
    private readonly List<InputActionMap> _actionMaps = new();

    public InputContext(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public IReadOnlyList<InputActionMap> ActionMaps => _actionMaps;

    public void AddActionMap(InputActionMap actionMap)
    {
        _actionMaps.Add(actionMap);
    }

    public void Enable()
    {
        foreach (var actionMap in _actionMaps)
        {
            actionMap.Enable();
        }
    }

    public void Disable()
    {
        foreach (var actionMap in _actionMaps)
        {
            actionMap.Disable();
        }
    }
}
