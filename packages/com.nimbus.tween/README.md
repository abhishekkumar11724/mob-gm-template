# com.nimbus.tween

Animation helper wrapper leveraging LitMotion.

## Overview

An abstraction over animation engines (LitMotion in v1). It insulates game logic from tween engine particulars while providing a `UIAnimations` preset library and strongly-typed sequence builders.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `0.0.0` | Required |
| `LitMotion` | `v2.0.1` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.tween": "file:../../packages/com.nimbus.tween"
  }
}
```

## Quick Start

```csharp
transform.TweenScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
UIAnimations.Pop(buttonRect);
```

## Key Types

| Type | Description |
|---|---|
| `UIAnimations` | Preset library for quick visual effects. |
| Extension methods | Extension logic targeting `Transform`, `RectTransform`, etc. |

## Architecture

UI layer accessory. Can be referenced widely without leaking low-level motion data.

## Configuration

Does not require custom SOs. Simply invoke.

## Events

| Event | Published When |
|---|---|
| (None) |  |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
