# com.nimbus.haptics

Cross-platform haptic feedback orchestrator.

## Overview

Facilitates mobile vibrations via pre-canned `HapticPattern` definitions, bridging iOS Taptic Engine and Android Vibrator gracefully. Configurable as a save-persisted toggle for users.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.haptics": "file:../../packages/com.nimbus.haptics"
  }
}
```

## Quick Start

```csharp
IHapticsService haptics = Services.Get<IHapticsService>();
haptics.Play(HapticPattern.Success);
```

## Key Types

| Type | Description |
|---|---|
| `IHapticsService` | Executes logic directly calling java/swift hardware nodes. |
| `HapticPattern` | Enumerates core options (Light, Medium, Heavy, Success, Warning, Failure, Selection). |

## Architecture

Platform layer directly operated by UX elements across UI contexts.

## Configuration

Default runtime checks for toggles. None generally required.

## Events

| Event | Published When |
|---|---|
| (None) |  |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
