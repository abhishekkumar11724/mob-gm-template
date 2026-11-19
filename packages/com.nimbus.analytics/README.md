# com.nimbus.analytics

Analytics provider abstraction featuring standard events and multi-provider fan-out.

## Overview

Decouples the game from SDK specifics, emitting strongly typed telemetry to one or more providers. Integrates directly with the `consent` package for privacy compliance.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.analytics": "file:../../packages/com.nimbus.analytics"
  }
}
```

## Quick Start

```csharp
IAnalyticsService analytics = Services.Get<IAnalyticsService>();
analytics.LogEvent(StandardEvents.LevelComplete(level: 1, stars: 3, time: 45.5f));
```

## Key Types

| Type | Description |
|---|---|
| `IAnalyticsService` | Central fan out service bridging standard payloads. |
| `StandardEvents` | Highly optimized registry creating allocation-less payload dicts. |

## Architecture

Data layer package designed as a singleton event bus routing system.

## Configuration

Add concrete SDK handler integrations (like Firebase) to your bootloader registry.

## Events

| Event | Published When |
|---|---|
| (None) |  |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
