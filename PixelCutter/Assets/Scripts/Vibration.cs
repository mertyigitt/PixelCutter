using UnityEngine;

public static class Vibration
{

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass UnityPlayer;
    public static AndroidJavaObject CurrentActivity;
    public static AndroidJavaObject Vibrator;
#endif

    public static void Vibrate()
    {
        if (İsAndroid())
            Vibrator.Call("vibrate");
    }


    public static void Vibrate(long milliseconds)
    {
        if (İsAndroid())
            Vibrator.Call("vibrate", milliseconds);
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (İsAndroid())
            Vibrator.Call("vibrate", pattern, repeat);
    }

    public static bool HasVibrator()
    {
        return İsAndroid();
    }

    public static void Cancel()
    {
        if (İsAndroid())
            Vibrator.Call("cancel");
    }

    private static bool İsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
}
