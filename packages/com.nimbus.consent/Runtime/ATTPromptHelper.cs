using System;
using Cysharp.Threading.Tasks;

#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace Nimbus.Consent;

public static class ATTPromptHelper
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern int Nimbus_GetATTStatus();

    [DllImport("__Internal")]
    private static extern void Nimbus_RequestATT();
#endif

    public static ATTStatus GetStatus()
    {
#if UNITY_IOS && !UNITY_EDITOR
        return (ATTStatus)Nimbus_GetATTStatus();
#else
        return ATTStatus.NotSupported;
#endif
    }

    public static async UniTask<ATTStatus> RequestAsync()
    {
#if UNITY_IOS && !UNITY_EDITOR
        if (GetStatus() != ATTStatus.NotDetermined)
        {
            return GetStatus();
        }

        Nimbus_RequestATT();

        while (GetStatus() == ATTStatus.NotDetermined)
        {
            await UniTask.Yield();
        }

        return GetStatus();
#else
        return await UniTask.FromResult(ATTStatus.NotSupported);
#endif
    }
}
