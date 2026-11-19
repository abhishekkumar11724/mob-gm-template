# com.nimbus.ui

UI framework featuring a screen stack, priority popup queue, and safe area fitting.

## Overview

Supplies a standard `ViewBase` for all UI panels. By dividing UI into stacked `ScreenView` and priority-queued `PopupView`, it prevents UI overlap and handles Android back navigation globally.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |
| `com.nimbus.tween` | `1.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.ui": "file:../../packages/com.nimbus.ui"
  }
}
```

## Quick Start

```csharp
// Example request for a prioritized popup
Services.Get<IUIService>().EnqueuePopup(new PopupRequest(myPopupId, PopupPriority.Normal));
```

## Key Types

| Type | Description |
|---|---|
| `IUIService` | Core screen stack & queue driver. |
| `ViewBase` | Abstract behaviour housing Show(), Hide() and basic MVVM setups. |
| `ScreenView` | Single-active background layer canvas handler. |
| `PopupView` | Piled views handled using a priority queue. |

## Architecture

UI layer package. Feature packages build their UI leveraging these primitives.

## Configuration

Attach standard prefabs to Addressable handles to trigger loading dynamically.

## Events

| Event | Published When |
|---|---|
| (None) | Relies on internal MVVM subscriptions inside game code. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
