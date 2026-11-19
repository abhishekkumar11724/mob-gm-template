# com.nimbus.analytics.firebase

Firebase Analytics implementation for the analytics abstraction.

## Overview

A concrete `IAnalyticsProvider` utilizing the Firebase native SDK to dispatch analytics. Properly transforms and limits payloads down to Firebase string requirement sets.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |
| `com.nimbus.analytics` | `1.0.0` | Required |
| `Firebase Analytics SDK` | (External) | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.analytics.firebase": "file:../../packages/com.nimbus.analytics.firebase"
  }
}
```

## Quick Start

```csharp
// Bootup auto registers component if included in assembly.
```

## Key Types

| Type | Description |
|---|---|
| (Internal implementor instances only) |  |

## Architecture

Integration layer.

## Configuration

Valid `google-services.json` and `GoogleService-Info.plist` files needed per Unity Firebase requirements.

## Events

| Event | Published When |
|---|---|
| (None) |  |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
