# com.nimbus.audio

Central audio management with music, SFX, banks, ducking, and volume persistence.

## Overview

The `audio` package provides a ServiceLocator-compatible API to play and manage sound effects and music. Rather than directly referencing audio clips, game logic uses predefined `AudioBankSO` assets, facilitating an abstracted audio pipeline.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.audio": "file:../../packages/com.nimbus.audio"
  }
}
```

## Quick Start

```csharp
IAudioService audio = Services.Get<IAudioService>();
audio.PlaySfx(SfxId.CoinPickup);
audio.PlayMusic(musicTrack, fadeSeconds: 1.0f);
```

## Key Types

| Type | Description |
|---|---|
| `IAudioService` | Core service for all audio operations. |
| `AudioBankSO` | Grouped audio clips loadable/unloadable together. |
| `MusicTrackSO` | Definition for a streaming or looping music track. |

## Architecture

Platform layer package. Depends on `core`.

## Configuration

Define `AudioBankSO` assets holding your audio clips and register them via Addressables.

## Events

| Event | Published When |
|---|---|
| (None) | Custom events rarely necessary. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
