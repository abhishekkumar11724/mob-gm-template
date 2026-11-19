# com.nimbus.remote-config.firebase

Firebase implementation for Nimbus Remote Config.

## Overview

The remote config firebase package provides a Firebase-backed implementation of the `IRemoteConfigProvider` interface defined in the `com.nimbus.remote-config` package. It enables fetching remote overrides and experiments from Firebase Remote Config.

## Dependencies

| Package | Version | Notes |
|---|---|---|
| `com.nimbus.remote-config` | `0.0.0` | Required |

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nimbus.remote-config.firebase": "file:../../packages/com.nimbus.remote-config.firebase"
  }
}
```

## Quick Start

```csharp
// Example of configuring the Firebase provider
var remoteConfig = Services.Get<IRemoteConfigService>();
remoteConfig.AddProvider(new FirebaseRemoteConfigProvider());
```

## Key Types

| Type | Description |
|---|---|
| `FirebaseRemoteConfigProvider` | Provider implementation that interacts with the Firebase SDK. |

## Architecture

The remote config firebase package is an integration package that depends on the `remote-config` abstraction. It maps Firebase Remote Config results into the JSON format expected by the `remote-config` service for merging with `GameConfigSO` assets.

## Configuration

Requires the Firebase Unity SDK (Remote Config) to be installed in the project. The provider uses the default Firebase App unless otherwise specified.

## Events

This package publishes no unique events but triggers `FetchCompleted` or `FetchFailed` on the `IRemoteConfigService`.

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

See [LICENSE.md](LICENSE.md).
