using System;

[Serializable]
public class AudioVolume<T> where T : SettingsManager
{
    public float MusicVolume;
    public float SoundVolume;
    public bool SetMute;
}
