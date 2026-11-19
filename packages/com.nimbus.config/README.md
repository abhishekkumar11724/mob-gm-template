# com.nimbus.config

ScriptableObject-based configuration with remote override merging and editor tooling.

## Overview

The `config` package provides a robust system for managing game configuration using Unity's ScriptableObjects. It lives in the Foundation layer and allows game logic to rely on typed SOs while supporting remote override merging (e.g., from Firebase Remote Config).

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.config": "file:../../packages/com.nimbus.config"
  }
}
```

## Quick Start

```csharp
// Define a config
[CreateAssetMenu]
public class BalanceConfigSO : GameConfigSO
{
    [RemoteOverridable]
    public float EnemySpeed = 1.0f;
}

// Access the config
BalanceConfigSO balance = Services.Get<ConfigRegistry>().GetConfig<BalanceConfigSO>();
```

## Key Types

| Type | Description |
|---|---|
| `GameConfigSO` | Base class for all configurations. |
| `[RemoteOverridable]` | Attribute to mark fields as remote-tunable. |
| `ConfigRegistry` | Service for discovering and indexing configs via Addressables. |
| `ConfigMerger` | Utility for applying remote JSON overrides to SOs. |

## Architecture

Foundation package. Depends on `core`. Other feature packages depend on it to define their configurations.

## Configuration

Create derived `GameConfigSO` assets and label them with `config` in Addressables.

## Events

| Event | Published When |
|---|---|
| (None) | No package-specific events. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
