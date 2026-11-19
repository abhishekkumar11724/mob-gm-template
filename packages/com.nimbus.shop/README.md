# com.nimbus.shop

Shop UI and catalog framework.

## Overview

The shop package provides a framework for managing virtual shops, products, and catalogs. It coordinates between the economy package for in-game currency purchases and the IAP package for real-money purchases. It lives in the feature layer of the architecture.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.core` | `0.0.0` | Required |
| `com.nimbus.config` | `0.0.0` | Required |
| `com.nimbus.economy` | `0.0.0` | Required |
| `com.nimbus.iap` | `0.0.0` | Required |
| `com.nimbus.ui` | `0.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.shop": "file:../../packages/com.nimbus.shop"
  }
}
```

## Quick Start

```csharp
var shopService = Services.Get<IShopService>();
var catalog = shopService.GetCatalog();
```

## Key Types

| Type | Description |
|---|---|
| `IShopService` | Primary service interface for the shop. |
| `ShopCatalogSO` | ScriptableObject defining the list of shop items. |
| `ShopItemSO` | Definition of a single shop item (price, reward, rules). |
| `ShopCategoryDefinitionSO` | Grouping definition for shop UI. |

## Architecture

The shop package depends on `core`, `config`, `economy`, `iap`, and `ui` packages. It coordinates purchases by delegating to `IEconomyService` or `IIAPService` depending on the item cost type.

## Configuration

The shop is configured via `ShopCatalogSO` assets which contain references to `ShopItemSO` assets. These assets define pricing, rewards, and availability rules.

## Events

This package currently does not publish its own events in Section 8.15.

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
