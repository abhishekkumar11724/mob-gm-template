using System.Text;
using Nimbus.Core;
using UnityEngine;
using UnityEngine.Profiling;

namespace Nimbus.DebugTools;

#if UNITY_EDITOR || DEVELOPMENT_BUILD || NIMBUS_ENABLE_DEBUG_TOOLS
[AddComponentMenu("Nimbus/Debug/Debug HUD")]
public sealed class DebugHud : MonoBehaviour, ITickable
{
    private const float UpdateInterval = 0.5f;

    private float _accumulatedTime;
    private int _frameCount;
    private float _currentFps;
    private long _reservedMemory;
    private long _allocatedMemory;

    private readonly StringBuilder _stringBuilder = new(128);
    private GUIStyle _style = null!;

    private void Awake()
    {
        _style = new GUIStyle();
        _style.fontSize = 20;
        _style.normal.textColor = Color.white;
        _style.padding = new RectOffset(10, 10, 10, 10);

        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(0, 0, 0, 0.5f));
        texture.Apply();
        _style.normal.background = texture;

        if (Services.TryGet<ITickerService>(out var ticker))
        {
            ticker.Add(this);
        }
    }

    private void OnDestroy()
    {
        if (Services.TryGet<ITickerService>(out var ticker))
        {
            ticker.Remove(this);
        }
    }

    public void Tick(float deltaTime)
    {
        _accumulatedTime += deltaTime;
        _frameCount++;

        if (_accumulatedTime >= UpdateInterval)
        {
            _currentFps = _frameCount / _accumulatedTime;
            _accumulatedTime = 0;
            _frameCount = 0;

            _reservedMemory = Profiler.GetTotalReservedMemoryLong();
            _allocatedMemory = Profiler.GetTotalAllocatedMemoryLong();
        }
    }

    private void OnGUI()
    {
        _stringBuilder.Clear();
        _stringBuilder.Append("FPS: ").Append(_currentFps.ToString("F1")).AppendLine();
        _stringBuilder.Append("Mem: ").Append((_allocatedMemory / 1024 / 1024).ToString()).Append("MB / ").Append((_reservedMemory / 1024 / 1024).ToString()).Append("MB");

        var content = new GUIContent(_stringBuilder.ToString());
        var size = _style.CalcSize(content);
        
        GUI.Label(new Rect(10, 10, size.x, size.y), content, _style);
    }
}
#else
public sealed class DebugHud : MonoBehaviour
{
}
#endif
