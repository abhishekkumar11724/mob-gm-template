# com.nimbus.consent

GDPR, CCPA, and ATT consent tracking and UI abstraction.

## Overview

A critical compliance mechanism triggering during Bootstrap Phase 3. It prompts for CMP interaction (like UMP) and informs all advertising and analytics systems of user permissions.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |
| `com.nimbus.ui` | `1.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.consent": "file:../../packages/com.nimbus.consent"
  }
}
```

## Quick Start

```csharp
IConsentService consent = Services.Get<IConsentService>();
bool canShowAds = consent.HasConsent(ConsentPurpose.Advertising);
```

## Key Types

| Type | Description |
|---|---|
| `IConsentService` | Check status of diverse data restriction permissions instantly. |
| `ConsentPurpose` | Categorical Enum distinguishing between Personalized Ads vs Base Analytics. |

## Architecture

Social & Compliance layer block executed primarily very early in boot cycles.

## Configuration

Settings dictate which geolocations auto-open specific screens.

## Events

| Event | Published When |
|---|---|
| `ConsentChanged` | Used by subsystems internally to hot-reload ad placements upon change settings operations. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
