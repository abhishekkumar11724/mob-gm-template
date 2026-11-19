# com.nimbus.ads.applovin

AppLovin MAX implementation for the ads abstraction.

## Overview

Provides an `IAdProvider` concrete implementation using the AppLovin MAX SDK. It reads user consent state natively before initializing or serving ads.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |
| `com.nimbus.ads` | `1.0.0` | Required |
| `AppLovin MAX SDK` | (External) | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.ads.applovin": "file:../../packages/com.nimbus.ads.applovin"
  }
}
```

## Quick Start

```csharp
// Implementation logic runs automatically during Bootstrap Phase 5.
```

## Key Types

| Type | Description |
|---|---|
| `IAdProvider` | Serves as the implementation shell over AppLovin classes. |

## Architecture

Integration layer package. Invisible to traditional app design files.

## Configuration

Relies entirely on standard AppLovin MAX project settings configuration setups provided in Unity Editor.

## Events

| Event | Published When |
|---|---|
| (None) | Forwards natively into `com.nimbus.ads` base system. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
