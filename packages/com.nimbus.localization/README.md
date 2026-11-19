# com.nimbus.localization

Key-based localization with CSV/JSON import pipeline.

## Overview

Simplifies text localization by strictly decoupling keys from strings. Features dynamic TMP_Text bindings and standard offline pipelines via Unity Editor tooling for CSV/JSON sources.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.localization": "file:../../packages/com.nimbus.localization"
  }
}
```

## Quick Start

```csharp
ILocalizationService loc = Services.Get<ILocalizationService>();
string text = loc.GetText(new LocKey("ui_start_button"));
```

## Key Types

| Type | Description |
|---|---|
| `ILocalizationService` | Exposes string retrieval matching the current device language. |
| `LocKey` | Strictly-typed wrapper to avoid raw string parsing issues. |
| `LocalizedText` | MonoBehavior attaching `LocKey` to `TMP_Text` displays. |

## Architecture

UI layer. Exclusively driven by asset catalogs configured ahead of time.

## Configuration

CSV pipelines drop items correctly formatted into localized `LocalizationTableSO` asset arrays.

## Events

| Event | Published When |
|---|---|
| `LanguageChanged` | Fired when moving between translation sets so UI can rebound instantaneously. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
