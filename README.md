# Hybrid Casual Unity Template

A modular package-based framework for shipping hybrid casual mobile games on Unity 6.

> **Picking this project up?** Read [docs/HANDOFF.md](docs/HANDOFF.md) first: current state, how builds work, the issues hit so far, and open items.

## What This Is

A **reusable foundation** for new hybrid casual games. It provides infrastructure — ads, IAP, analytics, save system, remote config, UI flow, and more — as a set of Unity Package Manager packages. Game code stays clean and focused on gameplay.

## Repo Structure

```
hybrid-casual-template/
├── README.md             This file.
│
├── .gitignore            Unity + IDE ignores.
├── .gitattributes        Unity YAML merge + Git LFS bindings.
├── .editorconfig         C# formatting enforced by IDE and CI.
├── Directory.Build.props MSBuild defaults for all packages.
│
├── packages/             All UPM packages developed here.
│   └── com.nimbus.*/
│
├── tools/                CI scripts and shared package templates.
│
└── sample-game/          Unity 6 project that consumes the packages.
    ├── Assets/
    ├── Packages/
    └── ProjectSettings/
```

## Prerequisites

- **Unity 6** (via Unity Hub)
- **Git** with Git LFS installed (`git lfs install` once per machine)
- **.NET SDK 8+** for editor tooling and analyzers

## First-Time Setup

### 1. Clone the repo

```
git clone <repo-url>
cd hybrid-casual-template
git lfs pull
```

### 2. Create the sample Unity project

The `sample-game/` folder is empty on first clone. Create the Unity project inside it:

- Open Unity Hub.
- New Project → Unity 6 → 2D or 3D template.
- Location: point at the repo's `sample-game/` folder.
- Project name: `sample-game`.
- Create. Unity generates `Assets/`, `Packages/`, `ProjectSettings/`, etc.

### 3. Configure Unity project settings

Once the project opens:

- Edit → Project Settings → Editor:
  - Version Control → Mode: **Visible Meta Files**
  - Asset Serialization → Mode: **Force Text**
  - Default Behaviour Mode → 2D or 3D per your choice.
- Save the scene, close Unity, commit the generated files.

### 4. Verify `.gitignore` is working

```
git status
```

You should see the newly created Unity files (Assets, Packages, ProjectSettings). You should **not** see `Library/`, `Temp/`, `Logs/`, or `.csproj` files. If you do, the `.gitignore` is not applied.

## Packages

Each package under `packages/com.nimbus.*` is a standalone Unity Package Manager package with its own `package.json`, assembly definitions, and runtime/editor split.

Highlights:

- **core** — service locator, event bus, lifecycle.
- **save** — multi-profile persistence with delayed slice hydration.
- **ads / ads.applovin** — ad provider abstraction with AppLovin adapter.
- **iap / iap.unity** — purchase flow with Unity IAP adapter.
- **analytics / analytics.firebase** — analytics abstraction with Firebase adapter.
- **remote-config / remote-config.firebase** — runtime config with Firebase adapter.
- **economy, progression, quests, daily-rewards, shop** — game economy systems.
- **ui, scene, tween, audio, haptics, input, localization, notifications, consent, attribution, social, tutorial, debug-tools** — supporting infrastructure.

## Naming Conventions

The template's `GOC` placeholder has been replaced with the studio name **Nimbus**:

- Namespaces and assembly names: `Nimbus.*` (e.g. `Nimbus.Core`).
- Package IDs and folders: `com.nimbus.*` (lowercase, as Unity Package Manager requires).
- Scripting defines: `NIMBUS_ENABLE_*`.
- Native iOS plugin entry points: `Nimbus_*`.

Affected files: all `package.json` files, asmdefs, namespaces, and the iOS plugins.

## Unity Package Manifest

The sample game's `sample-game/Packages/manifest.json` references each package via local path:

```json
{
  "dependencies": {
    "com.nimbus.core": "file:../../packages/com.nimbus.core",
    ...
  }
}
```

Paths are relative to `sample-game/Packages/`.

## Git LFS

Binary assets (images, audio, models, videos) are tracked via Git LFS per `.gitattributes`. Always run `git lfs pull` after cloning or switching branches — otherwise asset files will be text pointer stubs, not actual content.

Plain-text asset files (`.meta`, `.unity`, `.prefab`, `.mat`, `.asset`, `.asmdef`) are **not** in LFS. They stay as normal Git files so merges and diffs work correctly.

## Building

Unity compiles the game (Roslyn for C#, IL2CPP + the bundled NDK for Android); the .NET SDK is only for IDE tooling and `dotnet format`. Build entry points live in `sample-game/Assets/_Project/Editor/BuildRunner.cs`.

Android (APK, IL2CPP, ARM64, min SDK 26):

```bash
~/Unity/Hub/Editor/6000.6.9f1/Editor/Unity -batchmode -nographics -quit \
  -projectPath sample-game -buildTarget Android -logFile build-android.log \
  -executeMethod Nimbus.Build.BuildRunner.BuildAndroid
```

Output goes to `sample-game/Builds/Android/NimbusSample.apk`. Optional flags: `-buildAab` (App Bundle for Play Store), `-development`, `-outputPath <dir>`. Unity must not have the project open.

- **Signing:** with no keystore configured the APK is signed with Unity's debug keystore (fine for sideloading, not for Play Store). For release, export `NIMBUS_KEYSTORE_PATH`, `NIMBUS_KEYSTORE_PASS`, `NIMBUS_KEY_ALIAS` and `NIMBUS_KEY_PASS` before building. Never commit the keystore.
- **Browser (WebGL):** no device needed. Build with `-buildTarget WebGL -executeMethod Nimbus.Build.BuildRunner.BuildWebGL` (add `-development` for a faster, unoptimised build), then run `tools/serve-webgl.sh` and open the printed URL on this machine or on a phone on the same Wi-Fi. Output is uncompressed so any static server works. Android-only features (haptics, native ads, IAP, notifications) do not run in a browser. For the fastest edit loop, use Play mode with Window → General → Device Simulator instead of a build.
- **iOS:** `BuildRunner.BuildiOS` exports an Xcode project to `Builds/iOS/`. Producing an `.ipa` requires macOS with Xcode and the Unity iOS module, so it cannot be done on Linux.
- **CI:** `.github/workflows/ci.yml` runs the same `BuildRunner` methods through `game-ci/unity-builder`.

## Code Style

- 4-space indentation, LF endings, Allman braces.
- File-scoped namespaces, one public type per file.
- `_camelCase` private fields, `[SerializeField]` for inspector exposure.
- Methods ≤ 40 lines, classes ≤ 400 lines, nesting ≤ 3 levels.
- Nullable enabled at assembly level.
- UniTask for async; no `async void` outside event handlers.
- No singletons — services accessed via `Services.Get<IFoo>()`.

## Never Commit

- Unity `Library/` or `Temp/` folders.
- IDE folders (`.vs/`, `.idea/`, etc.).
- Build outputs (`.apk`, `.aab`, `Build/`).
- Keystores, API keys, secrets, `.env` files.
- Personal config (`UserSettings/`).

All of these are in `.gitignore`.

## Setup Notes (Unity 6000.6.9f1)

Changes made when bringing the template up on Unity 6000.6.9f1. `sample-game/` imports and compiles with 0 errors.

- **Compiler options:** `sample-game/Assets/csc.rsp` sets `-langversion:latest` (file-scoped namespaces need C# 10+; Unity defaults to C# 9) and `-nullable:enable` (Unity ignores `Directory.Build.props`).
- **Pins changed:** UniTask `2.5.11` and LitMotion `v2.0.2` (older releases use `TreeView` editor APIs that are hard errors in Unity 6000.6). NSubstitute is `v5.1.0` (the tag has a `v` prefix; `5.1.0` does not exist).
- **Git-URL dependencies** (UniTask, LitMotion, NSubstitute) are also listed in `sample-game/Packages/manifest.json`, because Package Manager does not resolve git-URL dependencies transitively.
- **Scoped registries** in the project manifest: AppLovin MAX (`com.applovin.mediation.ads` 8.6.2, from `https://unity.packages.applovin.com/`; replaces the old `com.applovin.max` git URL, which has no `package.json`) and OpenUPM, scoped to `com.google.external-dependency-manager` (EDM4U 1.2.189).
- **Asmdef fixes:** 11 runtime assemblies now reference `UniTask`; `Config`, `Scene` and `UI` also reference `UniTask.Addressables` (without it, `await` on an Addressables handle resolves to a `void` overload); `Scene` references `UI` (and `com.nimbus.ui` is a declared dependency).
- **New type:** `AssetReferenceScene` (`com.nimbus.scene`) was referenced but never defined.
- **Firebase:** the Firebase Unity SDK is not available from any UPM registry or git URL (it ships as `.unitypackage` files), so `com.nimbus.analytics.firebase` and `com.nimbus.remote-config.firebase` no longer declare `com.google.firebase.*` dependencies. Those adapters currently contain no code. Import the SDK into `sample-game/Assets/` when the adapters are implemented.
- **Tests:** every package's `Tests/` folder contains asmdefs only; there are no tests yet.

## License

See `LICENSE` file at repo root.
