using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;

namespace Nimbus.Tutorial;

/// <summary>
/// Service responsible for running tutorial sequences and tracking completion.
/// </summary>
public interface ITutorialService : IService
{
    /// <summary>
    /// Checks if a specific tutorial step has been completed.
    /// </summary>
    /// <param name="stepId">The unique identifier of the step.</param>
    /// <returns>True if completed, false otherwise.</returns>
    bool IsStepCompleted(string stepId);

    /// <summary>
    /// Marks a specific tutorial step as completed.
    /// </summary>
    /// <param name="stepId">The unique identifier of the step.</param>
    void MarkStepCompleted(string stepId);

    /// <summary>
    /// Runs a single tutorial step using the registered presenter.
    /// </summary>
    /// <param name="step">The step to run.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task that completes when the step is finished.</returns>
    UniTask RunStepAsync(TutorialStepSO step, CancellationToken ct = default);

    /// <summary>
    /// Runs a sequence of tutorial steps in order.
    /// </summary>
    /// <param name="steps">The steps to run.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task that completes when all steps in the sequence are finished.</returns>
    UniTask RunSequenceAsync(System.Collections.Generic.IEnumerable<TutorialStepSO> steps, CancellationToken ct = default);

    /// <summary>
    /// Sets the presenter used to render tutorial steps.
    /// </summary>
    /// <param name="presenter">The presenter implementation.</param>
    void SetPresenter(ITutorialPresenter presenter);
}
