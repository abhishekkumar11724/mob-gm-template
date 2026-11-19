# com.nimbus.ads

Ad provider abstraction, frequency capping, and ad placements.

## Overview

Exposes an `IAdService` that safely triggers interstitial and rewarded video ads based on configurable placements and cooldowns. Provider implementations (AppLovin, AdMob) ship as separate integration packages.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |
| `com.nimbus.config` | `1.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.ads": "file:../../packages/com.nimbus.ads"
  }
}
```

## Quick Start

```csharp
IAdService ads = Services.Get<IAdService>();
ads.ShowRewarded(AdPlacement.DoubleCoins, onReward: () => { ... });
```

## Key Types

| Type | Description |
|---|---|
| `IAdProvider` | Internal API required by ad implementations. |
| `AdPlacementSO` | Named placement containing cooldowns and logic gates. |
| `IAdService` | Facing API validating restrictions. |

## Architecture

Monetization feature layer. Abstracts SDK logic to decouple core app code from networks entirely.

## Configuration

Utilizes `AdFrequencyConfigSO` remotely overridable files for dynamic revenue throttling.

## Events

| Event | Published When |
|---|---|
| `AdShown` | Fired precisely when visual takes over screen. |
| `AdFailed` | Network or rule interruption reporting. |
| `RewardedCompleted` | Guaranteed successful user-interaction flag. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
