# com.nimbus.notifications

Local and remote notifications.

## Overview

The notifications package provides a unified framework for scheduling local notifications and handling remote push notifications via FCM. It allows for template-based scheduling for daily rewards, streak warnings, and quest expiries. It lives in the integration layer of the architecture.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `0.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.notifications": "file:../../packages/com.nimbus.notifications"
  }
}
```

## Quick Start

```csharp
var notifService = Services.Get<INotificationService>();
notifService.Schedule(new NotificationRequest("Daily Reward", "Your daily reward is waiting!"));
```

## Key Types

| Type | Description |
|---|---|
| `INotificationService` | Primary service for scheduling and cancelling notifications. |
| `NotificationScheduleSO` | ScriptableObject defining notification templates. |

## Architecture

The notifications package depends on `core` for service discovery. It acts as an abstraction over platform-specific notification systems and 3rd party SDKs like Firebase Cloud Messaging.

## Configuration

Notifications are configured via `NotificationScheduleSO` assets. These define the triggers (time intervals, daily offsets) and content for automated notifications.

## Events

This package currently does not publish specific events in Section 8.26.

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
