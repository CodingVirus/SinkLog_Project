using System;
[Serializable]
public class ScreenMode <T> where T : SettingsManager
{
    public uint ScreenWidth;
    public uint ScreenHeight;
    public bool SetWindowed;
}
