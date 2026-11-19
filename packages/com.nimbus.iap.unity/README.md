# com.nimbus.iap.unity

Unity IAP implementation for the IAP abstraction.

## Overview

Provides a concrete `IIAPProvider` implementation bridging the `iap` package with Unity's official In-App Purchasing package.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |
| `com.nimbus.iap` | `1.0.0` | Required |
| `com.unity.purchasing` | (Unity) | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.iap.unity": "file:../../packages/com.nimbus.iap.unity"
  }
}
```

## Quick Start

```csharp
// Instantiated dynamically inside the IAP Service builder.
```

## Key Types

| Type | Description |
|---|---|
| (Internal implementor instances only) |  |

## Architecture

Integration layer piece exclusively operating at lower-level engine systems.

## Configuration

Setup Unity In-App Purchasing through the normal Unity Services window.

## Events

| Event | Published When |
|---|---|
| (None) |  |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
