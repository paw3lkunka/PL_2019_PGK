#if UNITY_EDITOR_WIN
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Runtime.InteropServices;

[InitializeOnLoad]
public class PlayModeSystemMusicUtility
{
    public const int KEYEVENTF_EXTENTEDKEY = 1;
    public const int KEYEVENTF_KEYUP = 0;
    public const int VK_MEDIA_NEXT_TRACK = 0xB0;// code to jump to next track
    public const int VK_MEDIA_PLAY_PAUSE = 0xB3;// code to play or pause a song
    public const int VK_MEDIA_STOP = 0xB2; // code to stop media
    public const int VK_MEDIA_PREV_TRACK = 0xB1;// code to jump to prev track

    private static bool utilityEnabled = false;

    static PlayModeSystemMusicUtility()
    {
        EditorApplication.playModeStateChanged += PlayModeCallback;
    }

    private static void PlayModeCallback(PlayModeStateChange state)
    {
        using (var stream = new StreamReader("Assets/GitBashHere/gitpath.txt"))
        {
            stream.ReadLine();
            utilityEnabled = stream.ReadLine() == "music_utility_enabled";
        }

        if (utilityEnabled)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    keybd_event(VK_MEDIA_STOP, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                    break;
            }
        }
    }

    [DllImport("user32.dll")]
    public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);
}
#endif
