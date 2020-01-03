﻿using UnityEngine;
using System.Collections;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif
    public static void Vibrate(long milliseconds)
    {
        if (isAndroid())
        {
            vibrator.Call("vibrate", milliseconds);
        }
        else
        {
            Handheld.Vibrate();
        }
    }
    public static void Vibrate(long[] pattern, int repeat)
    {
        if (isAndroid())
        {
            vibrator.Call("vibrate", pattern, repeat);
        }
        else
        {
            Handheld.Vibrate();
        }
    }
    public static void Cancel()
    {
        if (isAndroid())
        {
            vibrator.Call("cancel");
        }
    }

    public static bool isAndroid()
    {
#if UNITY_EDITOR
        return false;
#elif UNITY_ANDROID
            return true;
#else
            return false;
#endif
    }
}