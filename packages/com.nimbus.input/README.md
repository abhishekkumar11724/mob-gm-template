# com.nimbus.input

New Input System wrapper with gesture recognition and a stack-based context system.

## Overview

The `input` package wraps Unity's New Input System to supply context-aware inputs and common gesture recognizers. Its context stack allows menus and popups to cleanly override gameplay input.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |
| `com.unity.inputsystem` | (Unity) | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.input": "file:../../packages/com.nimbus.input"
  }
}
```

## Quick Start

```csharp
IInputService input = Services.Get<IInputService>();
input.PushContext("pause_menu");
input.OnTap += position => { ... };
```

## Key Types

| Type | Description |
|---|---|
| `IInputService` | Primary interface for context stacks and event dispatching. |
| `InputContext` | A named set of enabled action maps pushed onto the stack. |
| `IGestureRecognizer` | Generic abstraction for Tap, Swipe, Pinch, Drag recognition. |

## Architecture

Platform layer package. Consumed heavily by UI and gameplay.

## Configuration

Utilizes Unity's native `InputActionAsset` workflows combined with custom context bindings.

## Events

| Event | Published When |
|---|---|
| (None) | Input changes expose standard C# `action` events natively off the service. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
