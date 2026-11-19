using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
using System.IO;
#endif

namespace Nimbus.Consent.Editor;

public static class IOSPostBuildProcessor
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
#if UNITY_IOS
        if (target != BuildTarget.iOS)
        {
            return;
        }

        string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        PBXProject project = new PBXProject();
        project.ReadFromString(File.ReadAllText(projectPath));

        string targetGuid = project.GetUnityFrameworkTargetGuid();
        project.AddFrameworkToProject(targetGuid, "AppTrackingTransparency.framework", false);

        File.WriteAllText(projectPath, project.WriteToString());

        string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        PlistElementDict rootDict = plist.root;
        string trackingDescription = "Your data will be used to deliver personalized ads to you.";
        rootDict.SetString("NSUserTrackingUsageDescription", trackingDescription);

        plist.WriteToFile(plistPath);
#endif
    }
}
