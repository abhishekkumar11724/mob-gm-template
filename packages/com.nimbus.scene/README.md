# com.nimbus.scene

Async scene loading and persistent-scene pattern orchestration.

## Overview

Replaces string-based Unity scene loading with Addressables, GUID-based scene references, and transition queues. It enforces the persistent `Boot` scene pattern and manages loading screen lifetimes.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |
| `com.unity.addressables` | (Unity) | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.scene": "file:../../packages/com.nimbus.scene"
  }
}
```

## Quick Start

```csharp
ISceneService scene = Services.Get<ISceneService>();
scene.TransitionAsync(new SceneLoadRequest {
    Target = _levelScene,
    ShowLoadingScreen = true
});
```

## Key Types

| Type | Description |
|---|---|
| `ISceneService` | Executor for scene transitions. |
| `SceneReference` | Serializable GUID-based wrapper for Unity Scenes. |
| `ISceneTransitionHook` | Allows blocking tasks before/after loads. |

## Architecture

Platform layer package. Dependent on `Addressables` heavily.

## Configuration

Scenes must be tagged correctly in your Addressables group configuration.

## Events

| Event | Published When |
|---|---|
| (None) | Operations tracked by state machines internally. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
