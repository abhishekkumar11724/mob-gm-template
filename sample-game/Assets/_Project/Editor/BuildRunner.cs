using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Nimbus.Build;

/// <summary>
/// Headless build entry points, used by CI and by local command-line builds.
/// <code>
/// Unity -batchmode -quit -projectPath sample-game -buildTarget Android \
///       -executeMethod Nimbus.Build.BuildRunner.BuildAndroid [-buildAab] [-development] [-outputPath dir]
/// </code>
/// Android release signing is read from the environment (never from the repo):
/// NIMBUS_KEYSTORE_PATH, NIMBUS_KEYSTORE_PASS, NIMBUS_KEY_ALIAS, NIMBUS_KEY_PASS.
/// Without them Android builds are signed with Unity's debug keystore.
/// </summary>
public static class BuildRunner
{
    private const string DefaultOutputRoot = "Builds";
    private const string ArtifactName = "NimbusSample";

    public static void BuildAndroid()
    {
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        bool buildAab = HasFlag("-buildAab");
        EditorUserBuildSettings.buildAppBundle = buildAab;
        ApplyAndroidSigning();

        string extension = buildAab ? "aab" : "apk";
        Run(BuildTarget.Android, BuildTargetGroup.Android, $"Android/{ArtifactName}.{extension}");
    }

    public static void BuildiOS()
    {
        // iOS output is an Xcode project folder. Compiling it into an .ipa still requires macOS + Xcode.
        Run(BuildTarget.iOS, BuildTargetGroup.iOS, "iOS");
    }

    public static void BuildWebGL()
    {
        // Uncompressed output so any plain static file server can host it (no Content-Encoding headers needed).
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
#if UNITY_WEBGL
        // Quick local iteration: shorter wasm optimisation. Only set with -development.
        if (HasFlag("-development"))
        {
            UnityEditor.WebGL.UserBuildSettings.codeOptimization = UnityEditor.WebGL.WasmCodeOptimization.BuildTimes;
        }
#endif
        Run(BuildTarget.WebGL, BuildTargetGroup.WebGL, "WebGL");
    }

    private static void Run(BuildTarget target, BuildTargetGroup group, string relativeOutput)
    {
        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Fail("No enabled scenes in EditorBuildSettings. Add at least one scene to the build.");
            return;
        }

        // Relative to the Unity project, not the process working directory (CI runs from the repo root).
        string projectRoot = Path.GetDirectoryName(Application.dataPath) ?? Directory.GetCurrentDirectory();
        string outputRoot = GetArg("-outputPath") ?? Path.Combine(projectRoot, DefaultOutputRoot);
        string outputPath = Path.GetFullPath(Path.Combine(outputRoot, relativeOutput));
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? outputRoot);

        BuildOptions options = HasFlag("-development") ? BuildOptions.Development : BuildOptions.None;

        Debug.Log($"[BuildRunner] {target} ({group}) -> {outputPath}");
        Debug.Log($"[BuildRunner] Scenes: {string.Join(", ", scenes)}");

        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            target = target,
            targetGroup = group,
            options = options,
        });

        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"[BuildRunner] Succeeded: {summary.outputPath} ({summary.totalSize / (1024 * 1024)} MB, "
                + $"{summary.totalTime.TotalMinutes:F1} min, {summary.totalWarnings} warnings)");
            EditorApplication.Exit(0);
            return;
        }

        Fail($"Build {summary.result}: {summary.totalErrors} error(s). See the Unity log for details.");
    }

    private static void ApplyAndroidSigning()
    {
        string keystorePath = Environment.GetEnvironmentVariable("NIMBUS_KEYSTORE_PATH");
        if (string.IsNullOrEmpty(keystorePath))
        {
            Debug.Log("[BuildRunner] NIMBUS_KEYSTORE_PATH not set; signing with the Unity debug keystore.");
            PlayerSettings.Android.useCustomKeystore = false;
            return;
        }

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = Path.GetFullPath(keystorePath);
        PlayerSettings.Android.keystorePass = Environment.GetEnvironmentVariable("NIMBUS_KEYSTORE_PASS");
        PlayerSettings.Android.keyaliasName = Environment.GetEnvironmentVariable("NIMBUS_KEY_ALIAS");
        PlayerSettings.Android.keyaliasPass = Environment.GetEnvironmentVariable("NIMBUS_KEY_PASS");
        Debug.Log("[BuildRunner] Signing with custom keystore from environment.");
    }

    private static bool HasFlag(string name)
    {
        return Environment.GetCommandLineArgs().Contains(name);
    }

    private static string? GetArg(string name)
    {
        string[] args = Environment.GetCommandLineArgs();
        int index = Array.IndexOf(args, name);
        return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
    }

    private static void Fail(string message)
    {
        Debug.LogError($"[BuildRunner] {message}");
        EditorApplication.Exit(1);
    }
}
