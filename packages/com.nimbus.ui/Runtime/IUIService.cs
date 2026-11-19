using Cysharp.Threading.Tasks;
using Nimbus.Core;
using UnityEngine;

namespace Nimbus.UI;

/// <summary>
/// Orchestrates screen transitions and popup queuing.
/// </summary>
public interface IUIService : IService
{
    /// <summary>
    /// Sets the root transform where views will be instantiated.
    /// </summary>
    /// <param name="root">The root transform.</param>
    void SetRoot(Transform root);

    /// <summary>
    /// Shows a screen of the given type. 
    /// If another screen is active, it will be hidden first.
    /// </summary>
    /// <typeparam name="T">The type of the screen.</typeparam>
    /// <returns>The screen instance.</returns>
    UniTask<T> ShowScreenAsync<T>() where T : ScreenView;

    /// <summary>
    /// Hides the current screen if it matches the given type.
    /// If there was a screen below it in the stack, it will be shown.
    /// </summary>
    /// <typeparam name="T">The type of the screen.</typeparam>
    /// <returns>UniTask.</returns>
    UniTask HideScreenAsync<T>() where T : ScreenView;

    /// <summary>
    /// Enqueues a popup of the given type.
    /// Popups are shown one by one according to their priority.
    /// </summary>
    /// <typeparam name="T">The type of the popup.</typeparam>
    /// <param name="request">The popup request details.</param>
    /// <returns>The popup instance.</returns>
    UniTask<T> ShowPopupAsync<T>(PopupRequest request = default) where T : PopupView;

    /// <summary>
    /// Closes a specific popup and processes the next one in the queue.
    /// </summary>
    /// <param name="popup">The popup to close.</param>
    /// <returns>UniTask.</returns>
    UniTask ClosePopupAsync(PopupView popup);

    /// <summary>
    /// Clears the popup queue and closes any active popup.
    /// </summary>
    /// <returns>UniTask.</returns>
    UniTask CloseAllPopupsAsync();
}
