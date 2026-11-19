# com.nimbus.tutorial

Flexible tutorial framework for UI and gameplay tutorials.

## Overview

The tutorial package provides a robust mechanism for creating step-based onboarding and gameplay tutorials. It lives in the feature layer of the architecture, coordinating between configuration, save state, and UI to guide players through the game.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `0.0.0` | Required |
| `com.nimbus.save` | `0.0.0` | Required |
| `com.nimbus.ui` | `0.0.0` | Required |
| `com.nimbus.input` | `0.0.0` | Required |
| `com.nimbus.localization` | `0.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.tutorial": "file:../../packages/com.nimbus.tutorial"
  }
}
```

## Quick Start

```csharp
// Example of starting a tutorial sequence
var tutorialService = Services.Get<ITutorialService>();
tutorialService.StartSequence("onboarding_sequence");
```

## Key Types

| Type | Description |
|---|---|
| `ITutorialService` | Primary service for running tutorial sequences. |
| `TutorialStepSO` | ScriptableObject defining a single tutorial step. |
| `TutorialSequenceSO` | ScriptableObject defining an ordered list of tutorial steps. |
| `ITutorialCondition` | Interface for pluggable tutorial triggers and completion logic. |

## Architecture

The tutorial package is a feature package that depends on core and foundation packages for its functionality. It uses the input context stack to restrict player input during tutorials and the localization package for displayed text.

## Configuration

Tutorials are configured via `TutorialSequenceSO` and `TutorialStepSO` assets. These assets define the order of steps and the conditions for each step to trigger and complete.

## Events

| Event | Published When |
|---|---|
| `TutorialStarted` | When a tutorial sequence begins. |
| `TutorialStepStarted` | When a specific step in a sequence starts. |
| `TutorialStepCompleted` | When a specific step in a sequence is completed. |
| `TutorialSequenceCompleted` | When all steps in a sequence have been completed. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
