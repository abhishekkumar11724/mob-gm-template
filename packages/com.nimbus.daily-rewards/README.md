# com.nimbus.daily-rewards

Login streak rewards framework.

## Overview

The daily rewards package provides a flexible framework for managing login streaks and daily rewards. It coordinates between the configuration, save, and economy packages to distribute rewards based on a defined schedule and streak rules.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `0.0.0` | Required |
| `com.nimbus.config` | `0.0.0` | Required |
| `com.nimbus.save` | `0.0.0` | Required |
| `com.nimbus.economy` | `0.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.daily-rewards": "file:../../packages/com.nimbus.daily-rewards"
  }
}
```

## Quick Start

```csharp
var rewards = Services.Get<IDailyRewardService>();
if (rewards.CanClaim())
{
    rewards.Claim();
}
```

## Key Types

| Type | Description |
|---|---|
| `IDailyRewardService` | Primary service for checking and claiming daily rewards. |
| `DailyRewardScheduleSO` | ScriptableObject defining the ordered list of rewards per day. |

## Architecture

The daily rewards package is a feature package that depends on `core` for services, `config` for reward schedules, `save` for streak persistence, and `economy` for granting rewards.

## Configuration

Daily rewards are configured via `DailyRewardScheduleSO` assets. You can define streak-break rules such as resetting on miss, freezing the streak, or providing a grace period.

## Events

| Event | Published When |
|---|---|
| `DailyRewardClaimed` | When a daily reward is successfully claimed. |
| `DailyStreakBroken` | When a streak is broken according to the schedule rules. |
| `DailyStreakReset` | When a streak is explicitly reset. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
