# com.nimbus.save

Slice-based save system with versioning and cloud sync interface.

## Overview

The `save` package handles saving and loading game data using independent slices. Each feature owns its save shape to avoid monolithic migrations. It provides asynchronous, atomic write safety and file backups.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.save": "file:../../packages/com.nimbus.save"
  }
}
```

## Quick Start

```csharp
public sealed class PuzzleSaveSlice : ISaveSlice { ... }
Services.Get<SaveService>().RegisterSlice(new PuzzleSaveSlice(...));
```

## Key Types

| Type | Description |
|---|---|
| `ISaveSlice` | Interface for a single save data domain. |
| `SaveService` | Central orchestrator for the save cycle. |
| `ISaveStorage` | Abstraction over file system storage. |

## Architecture

Foundation package. Depends on `core`. Consumed by feature packages (economy, progression, etc.) to persist their state.

## Configuration

Configured during Bootstrap phase 2.

## Events

| Event | Published When |
|---|---|
| `SaveStarted` | Triggered when a save operation begins. |
| `SaveCompleted` | Triggered when a save succeeds. |
| `LoadCompleted` | Triggered when save files are done loading into memory. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
