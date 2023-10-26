using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolumeSet : MonoBehaviour
{
    private void Update()
    {
        GetComponent<AudioSource>().volume = SettingsManager.Instance.Audio.MusicVolume;
    }
}
