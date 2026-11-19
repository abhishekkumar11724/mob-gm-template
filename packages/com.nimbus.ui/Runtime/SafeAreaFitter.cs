using UnityEngine;

namespace Nimbus.UI;

[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("Nimbus/UI/Safe Area Fitter")]
public sealed class SafeAreaFitter : MonoBehaviour
{
    [SerializeField] private bool _fitTop = true;
    [SerializeField] private bool _fitBottom = true;
    [SerializeField] private bool _fitLeft = true;
    [SerializeField] private bool _fitRight = true;

    private RectTransform _rectTransform = null!;
    private Rect _lastSafeArea = Rect.zero;
    private Vector2 _lastScreenSize = Vector2.zero;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Rect currentSafeArea = Screen.safeArea;
        Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);

        if (currentSafeArea == _lastSafeArea && currentScreenSize == _lastScreenSize)
        {
            return;
        }

        _lastSafeArea = currentSafeArea;
        _lastScreenSize = currentScreenSize;

        ApplySafeArea(currentSafeArea);
    }

    private void ApplySafeArea(Rect safeArea)
    {
        float width = Screen.width;
        float height = Screen.height;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= width;
        anchorMin.y /= height;
        anchorMax.x /= width;
        anchorMax.y /= height;

        Vector2 currentMin = _rectTransform.anchorMin;
        Vector2 currentMax = _rectTransform.anchorMax;

        if (_fitLeft)
        {
            currentMin.x = anchorMin.x;
        }

        if (_fitBottom)
        {
            currentMin.y = anchorMin.y;
        }

        if (_fitRight)
        {
            currentMax.x = anchorMax.x;
        }

        if (_fitTop)
        {
            currentMax.y = anchorMax.y;
        }

        _rectTransform.anchorMin = currentMin;
        _rectTransform.anchorMax = currentMax;
    }
}
