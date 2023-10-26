using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{

    [SerializeField] private AudioSource bgm;

    void Start()
    {
        bgm.Play();
        bgm.loop = true;
    }

   
    void Update()
    {
        GetComponent<AudioSource>().volume = SettingsManager.Instance.Audio.MusicVolume;
    }
}
