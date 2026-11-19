# com.nimbus.debug-tools

Developer menu, FPS overlay, cheat actions, and log viewer.

## Overview

Offers runtime oversight tools. It acts as an integration hub for features to expose testing shims (e.g., adding arbitrary currency). Strip-compiles out of release builds.

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
    "com.nimbus.debug-tools": "file:../../packages/com.nimbus.debug-tools"
  }
}
```

## Quick Start

```csharp
IDebugService debug = Services.Get<IDebugService>();
debug.RegisterAction("Economy/Add 1000 Coins", () => ... );
```

## Key Types

| Type | Description |
|---|---|
| `IDebugService` | Endpoint collecting cross-package debug actions dynamically. |
| `DebugMenuView` | The core instantiated GUI component. |

## Architecture

Dev Tools layer interacting at the top to access states globally without polluting normal modules.

## Configuration

Compiled exclusively when `DEBUG` flags are supplied preventing shipping vulnerabilities.

## Events

| Event | Published When |
|---|---|
| (None) |  |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
