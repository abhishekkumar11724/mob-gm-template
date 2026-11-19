# com.nimbus.progression

Player level, XP, unlocks, and level completion data tracking.

## Overview

Tracks game progression independently of specific level details. Allows dynamic XP scaling via `XPCurveSO` and orchestrates gate conditions (UnlockRules).

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |
| `com.nimbus.config` | `1.0.0` | Required |
| `com.nimbus.save` | `1.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.progression": "file:../../packages/com.nimbus.progression"
  }
}
```

## Quick Start

```csharp
IProgressionService prog = Services.Get<IProgressionService>();
prog.AddXP(50);
```

## Key Types

| Type | Description |
|---|---|
| `IProgressionService` | Controls leveling boundaries and unlock evaluation. |
| `XPCurveSO` | Config detailing level/XP tables. |
| `UnlockRuleSO` | Pluggable rule system deciding feature accessibility checkpoints. |

## Architecture

Feature layer package. Defines pure logic that game components subscribe to.

## Configuration

Extensive logic stored within `XPCurveSO` rulesets.

## Events

| Event | Published When |
|---|---|
| `PlayerLeveledUp` | Fired upon rank changes. |
| `LevelCompleted` | Generic handler matching an end-of-stage situation. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
