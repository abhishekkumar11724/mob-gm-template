using System;
using Nimbus.Core;
using UnityEngine;

namespace Nimbus.Input;

public interface IInputService : IService
{
    void PushContext(string contextName);

    void PopContext();

    event Action<Vector2> OnTap;

    event Action<Vector2> OnSwipe;

    event Action<float> OnPinch;

    event Action<Vector2> OnLongPress;

    event Action<Vector2> OnDrag;
}
