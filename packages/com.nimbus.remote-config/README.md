# com.nimbus.remote-config

Remote config merging into local ScriptableObjects.

## Overview

Automates live-ops tunables without store updates. It hits a provider for JSON overrides, then utilizes `ConfigMerger` to update local `GameConfigSO` instances caching values when offline.

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
    "com.nimbus.remote-config": "file:../../packages/com.nimbus.remote-config"
  }
}
```

## Quick Start

```csharp
IRemoteConfigService remote = Services.Get<IRemoteConfigService>();
// Done silently usually on Boot via: remote.FetchAsync();
```

## Key Types

| Type | Description |
|---|---|
| `IRemoteConfigService` | Coordinator ensuring fetched strings mutate Addressable configurations safely. |

## Architecture

Data layer companion to the `config` foundation architecture element.

## Configuration

Retry limits and boot requirements exposed in Phase 4 parameters.

## Events

| Event | Published When |
|---|---|
| `FetchCompleted` | When networks successfully update payloads. |
| `ValuesApplied` | Follow-up ensuring properties are actively modifying games parameters now. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
