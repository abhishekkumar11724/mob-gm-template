# com.nimbus.core

Foundation primitives shared by every package.

## Overview

The `core` package provides the fundamental mechanisms for the Hybrid Casual Template, including the service locator, event bus, and lifecycle interfaces. It lives at the base of the dependency graph (Core layer) and depends only on Unity and selected third-party libraries.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `Newtonsoft JSON` | `3.2.1` | Built-in Unity package |
| `UniTask` | `2.5.0` | External dependency (v1 uses Git URL) |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.core": "file:../../packages/com.nimbus.core"
  }
}
```

## Quick Start

```csharp
// Registering a service
Services.Register<IMyService>(new MyServiceImpl());

// Getting a service
IMyService myService = Services.Get<IMyService>();

// Subscribing to an event
EventBus.Subscribe<CurrencyChangedEvent>(OnCurrencyChanged);

// Publishing an event
EventBus.Publish(new CurrencyChangedEvent(100));
```

## Key Types

| Type | Description |
|---|---|
| `ServiceLocator` | Interface-keyed service registry. |
| `EventBus` | Typed, synchronous, allocation-aware event bus. |
| `IInitializable` | Interface for services that require async setup. |
| `ITickable` | Interface for services that require per-frame updates. |

## Architecture

This package is the root of the dependency graph. Every other package in the template depends on `core`. It has zero dependencies on other `com.nimbus.*` packages.

## Configuration

Core services are generally configured through code during the Bootstrap sequence.

## Events

Shared core events are located in the `Nimbus.Core.Events` namespace.

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
