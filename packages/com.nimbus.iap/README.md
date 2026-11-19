# com.nimbus.iap

IAP abstraction handling catalogs, transactions, and receipt validation.

## Overview

A cross-platform framework for processing real-money purchases. It handles entitlements, restoring purchases, and tracking via its own save slice. Uses the `IReceiptValidator` abstraction for server validation.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `1.0.0` | Required |
| `com.nimbus.config` | `1.0.0` | Required |
| `com.nimbus.save` | `1.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.iap": "file:../../packages/com.nimbus.iap"
  }
}
```

## Quick Start

```csharp
IIAPService iap = Services.Get<IIAPService>();
iap.Purchase("com.studio.game.starter_pack");
```

## Key Types

| Type | Description |
|---|---|
| `IIAPProvider` | Core API implementation base. |
| `IAPProductSO` | Data structure tying string names to store objects. |

## Architecture

Monetization feature layer. Sits as the basis for the `shop` system.

## Configuration

Create comprehensive `IAPProductSO` structures inside your game project folders and load at Boot.

## Events

| Event | Published When |
|---|---|
| `PurchaseStarted` | Boot up callback. |
| `PurchaseCompleted` | Secure completion and network callback confirmation. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
