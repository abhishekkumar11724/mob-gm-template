using System.Threading;
using Cysharp.Threading.Tasks;

namespace Nimbus.Tutorial;

/// <summary>
/// Abstraction for rendering tutorial steps.
/// </summary>
public interface ITutorialPresenter
{
    /// <summary>
    /// Renders the specified tutorial step.
    /// </summary>
    /// <param name="step">The tutorial step to render.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task that completes when the step is shown.</returns>
    UniTask ShowAsync(TutorialStepSO step, CancellationToken ct);

    /// <summary>
    /// Hides any currently active tutorial step.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task that completes when the step is hidden.</returns>
    UniTask HideAsync(CancellationToken ct);
}
