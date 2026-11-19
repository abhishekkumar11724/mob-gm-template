# com.nimbus.economy

Currencies, items, inventory, and atomic multi-asset transactions.

## Overview

The central vault of the game. It controls currencies and item definitions while ensuring no partial-state failures via atomic multi-debit/credit transactions. Includes an `EconomySaveSlice` for persistence.

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
    "com.nimbus.economy": "file:../../packages/com.nimbus.economy"
  }
}
```

## Quick Start

```csharp
IEconomyService economy = Services.Get<IEconomyService>();
TransactionResult result = economy.TryPurchase(costCurrency, amount);
```

## Key Types

| Type | Description |
|---|---|
| `CurrencyDefinitionSO` | Global currency logic holder. |
| `IEconomyService` | Validates limits and provides balance reports. |
| `TransactionResult` | Struct ensuring precise tracking of wallet commits. |

## Architecture

Feature layer package. The shop logic builds off of this internally.

## Configuration

Requires `CurrencyDefinitionSO` and `ItemDefinitionSO` setup in Unity.

## Events

| Event | Published When |
|---|---|
| `CurrencyChanged` | On balance adjustment. |
| `TransactionCompleted` | When whole trades successfully commit. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
