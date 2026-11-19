# com.nimbus.quests

Quest framework for daily, weekly, and event quests.

## Overview

The quest package provides a flexible framework for managing player tasks that reset on various schedules (daily, weekly) or are tied to specific events. It lives in the feature layer of the architecture, coordinating between configuration, save state, and the economy package for rewards.

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
    "com.nimbus.quests": "file:../../packages/com.nimbus.quests"
  }
}
```

## Quick Start

```csharp
// Example of tracking a quest event
var questService = Services.Get<IQuestService>();
questService.Track(new QuestEvent("kill_enemy", 1));
```

## Key Types

| Type | Description |
|---|---|
| `IQuestService` | Primary service for managing and tracking quests. |
| `QuestDefinitionSO` | ScriptableObject defining quest requirements and rewards. |
| `IQuestObjective` | Interface for pluggable quest completion logic. |

## Architecture

The quest package depends on `core` for events and services, `config` for data definitions, `save` for persistence, and `economy` for reward distribution. It exposes a tracking API that can be called from any gameplay system.

## Configuration

Quests are defined as `QuestDefinitionSO` assets and placed in Addressables groups tagged with `config`. Each definition specifies the objective type, target count, reward (using economy definitions), and duration.

## Events

| Event | Published When |
|---|---|
| `QuestStarted` | When a new quest is added to the active list. |
| `QuestProgressChanged` | When progress is made on an active quest. |
| `QuestCompleted` | When a quest's target count is reached. |
| `QuestClaimed` | When rewards for a completed quest are granted. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
