using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Nimbus.Input;

public class TouchSimulationHelper : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            EnhancedTouchSupport.Enable();
            TouchSimulation.Enable();
        }
    }

    private void OnDisable()
    {
        if (Application.isPlaying)
        {
            TouchSimulation.Disable();
            EnhancedTouchSupport.Disable();
        }
    }
#endif
}
