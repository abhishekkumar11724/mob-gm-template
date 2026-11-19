# com.nimbus.attribution

Install attribution and deep linking.

## Overview

The attribution package provides an abstraction for install attribution and deep linking. It allows the game to coordinate campaign data from providers like AppsFlyer or Adjust and resolve deep links at app start and during sessions. It lives in the integration layer of the architecture.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `0.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.attribution": "file:../../packages/com.nimbus.attribution"
  }
}
```

## Quick Start

```csharp
var attribution = Services.Get<IAttributionService>();
attribution.OnInstallAttributed += (data) => {
    Log.Attribution.Info($"Campaign: {data.Campaign}");
};
```

## Key Types

| Type | Description |
|---|---|
| `IAttributionProvider` | Abstraction for attribution SDKs (AppsFlyer, Adjust, etc). |
| `IAttributionService` | Game-facing service for attribution data. |
| `IDeepLinkHandler` | Resolves and routes deep links. |

## Architecture

The attribution package depends on `core` for service discovery and events. It integrates with the analytics package by setting user properties from campaign data when attribution is received.

## Configuration

Providers are configured via provider-specific ScriptableObjects or SDK settings. The `IAttributionService` coordinates between active providers.

## Events

| Event | Published When |
|---|---|
| `InstallAttributed` | When campaign attribution data is successfully received from a provider. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
