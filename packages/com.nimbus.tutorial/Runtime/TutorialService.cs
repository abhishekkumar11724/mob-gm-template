using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;

namespace Nimbus.Tutorial;

/// <summary>
/// Implementation of the tutorial service.
/// </summary>
public sealed class TutorialService : ITutorialService
{
    private readonly HashSet<string> _completedSteps = new();
    private ITutorialPresenter? _presenter;

    public bool IsStepCompleted(string stepId)
    {
        if (string.IsNullOrEmpty(stepId))
        {
            throw new ArgumentException("Step ID cannot be null or empty.", nameof(stepId));
        }

        return _completedSteps.Contains(stepId);
    }

    public void MarkStepCompleted(string stepId)
    {
        if (string.IsNullOrEmpty(stepId))
        {
            throw new ArgumentException("Step ID cannot be null or empty.", nameof(stepId));
        }

        _completedSteps.Add(stepId);
    }

    public async UniTask RunStepAsync(TutorialStepSO step, CancellationToken ct = default)
    {
        if (step == null)
        {
            throw new ArgumentNullException(nameof(step));
        }

        if (_presenter == null)
        {
            Log.Tutorial.Error("Cannot run tutorial step: No presenter registered.");
            return;
        }

        await _presenter.ShowAsync(step, ct);
        MarkStepCompleted(step.Id);
        await _presenter.HideAsync(ct);
    }

    public async UniTask RunSequenceAsync(IEnumerable<TutorialStepSO> steps, CancellationToken ct = default)
    {
        if (steps == null)
        {
            throw new ArgumentNullException(nameof(steps));
        }

        foreach (var step in steps)
        {
            if (ct.IsCancellationRequested)
            {
                break;
            }

            await RunStepAsync(step, ct);
        }
    }

    public void SetPresenter(ITutorialPresenter presenter)
    {
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
    }
}
