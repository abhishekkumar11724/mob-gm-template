# com.nimbus.social

Social sharing, rate prompts, and referral framework.

## Overview

The social package provides a unified API for common social interactions such as sharing content, requesting app ratings, and generating referral links. It abstracts platform-specific implementations (iOS/Android) and integrates with attribution providers for deep-linked referrals. It lives in the integration layer of the architecture.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `0.0.0` | Required |
| `com.nimbus.attribution` | `0.0.0` | Required for referral links |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.social": "file:../../packages/com.nimbus.social"
  }
}
```

## Quick Start

```csharp
var social = Services.Get<ISocialService>();
social.Share("Check out my high score!", screenshotTexture);
```

## Key Types

| Type | Description |
|---|---|
| `ISocialService` | Primary service for social interactions. |

## Architecture

The social package depends on `core` for service discovery and `attribution` for deep link generation. It provides a common interface that delegates to native platform implementations or attribution SDKs.

## Configuration

Referral links are configured via the active attribution provider settings. Rate prompts are triggered through the `ISocialService` and may have session-gate rules.

## Events

This package currently does not publish specific events in Section 8.27.

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
