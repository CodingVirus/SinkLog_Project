using System;
using System.IO;
using UnityEngine;
        
[System.Serializable]
public class SettingsManager : Singleton<SettingsManager>
{
    [Serializable]
    [SerializeField] private class SettingSave
    {
        public KeyCode[] KeysA = new KeyCode[13];
        public KeyCode[] KeysB = new KeyCode[13];
        public float MouseSensitivity;

        public AudioVolume<SettingsManager> AudioSave = new AudioVolume<SettingsManager>();
        public ScreenMode<SettingsManager> ScreenSave = new ScreenMode<SettingsManager>();
    }

    private string filePath;


    // Current Set
    public InputKeys<SettingsManager> Inputkeys = new InputKeys<SettingsManager>();
    public AudioVolume<SettingsManager> Audio = new AudioVolume<SettingsManager>();
    public ScreenMode<SettingsManager> Screen = new ScreenMode<SettingsManager>();

    // For Save and Load
    private SettingSave mySettingSave = new SettingSave();

    private GlobalEnums.OptionStatus optionStatus = GlobalEnums.OptionStatus.None;
    public GlobalEnums.OptionStatus OptionStatus { get => optionStatus; }

    private void Start()
    {
        filePath = $"{Application.persistentDataPath}/OptionSave/OptionSaveFile.Json";
        FileInfo fileInfo = new FileInfo(filePath);
        if (fileInfo.Exists)
        {
            LoadOptionSetting();
            LoadKeySetting();
        }
        else
        {
            CreateOptionFile();
        }
    }


    public void CreateOptionFile()
    {
        Directory.CreateDirectory($"{Application.persistentDataPath}/OptionSave");
        ResetOptionSetting();
        ResetKeySetting();
        SaveOptionSetting();
        SaveKeySetting();
    }

    /// <summary>
    /// LoadOption(exclude KeySet)
    /// </summary>
    public void LoadOptionSetting()
    {
        var textAsset = File.ReadAllText(filePath);
        mySettingSave = JsonUtility.FromJson<SettingSave>(textAsset);
        Audio = mySettingSave.AudioSave;
        Screen = mySettingSave.ScreenSave;
        Inputkeys.MouseSensitivity = mySettingSave.MouseSensitivity;
    }

    /// <summary>
    /// LoadOption(include KeySet)
    /// </summary>
    public void LoadKeySetting()
    {
        var textAsset = File.ReadAllText(filePath);
        mySettingSave = JsonUtility.FromJson<SettingSave>(textAsset);
        for (int i = Inputkeys.keyDictionary.Count - 1; i >= 0; i--)
        {
            Inputkeys.keyDictionary[(GlobalEnums.InputKeys)i][0] = mySettingSave.KeysA[i];
            Inputkeys.keyDictionary[(GlobalEnums.InputKeys)i][1] = mySettingSave.KeysB[i];
        }
    }

    public void SaveKeySetting()
    {
        for (int i = Inputkeys.keyDictionary.Count - 1; i >= 0; i--)
        {
            mySettingSave.KeysA[i] = Inputkeys.keyDictionary[(GlobalEnums.InputKeys)i][0];
            mySettingSave.KeysB[i] = Inputkeys.keyDictionary[(GlobalEnums.InputKeys)i][1];
        }
        //LoadOptionSetting();
        FileInfo fileInfo = new FileInfo(filePath);
        File.WriteAllText(filePath, JsonUtility.ToJson(mySettingSave, true));
    }
    public void SaveOptionSetting()
    {
        //LoadKeySetting();
        mySettingSave.AudioSave = Audio;
        mySettingSave.ScreenSave = Screen;
        mySettingSave.MouseSensitivity = Inputkeys.MouseSensitivity;
        File.WriteAllText(filePath, JsonUtility.ToJson(mySettingSave, true));
    }

    public void ResetKeySetting()
    {
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)0][0] = KeyCode.D;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)0][1] = default;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)1][0] = KeyCode.A;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)1][1] = default;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)2][0] = KeyCode.Space;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)2][1] = default;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)3][0] = KeyCode.Mouse0;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)3][1] = default;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)4][0] = KeyCode.Mouse1;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)4][1] = default;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)5][0] = KeyCode.I;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)5][1] = KeyCode.B;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)6][0] = KeyCode.Tab;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)6][1] = default;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)7][0] = KeyCode.Alpha1;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)7][1] = default;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)8][0] = KeyCode.Alpha2;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)8][1] = default;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)9][0] = KeyCode.F;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)9][1] = default;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)10][0] = KeyCode.R;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)10][1] = default;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)11][0] = KeyCode.Q;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)11][1] = default;

        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)12][0] = KeyCode.E;
        Inputkeys.keyDictionary[(GlobalEnums.InputKeys)12][1] = default;

        Inputkeys.MouseSensitivity = 1.0f;
    }

    public void ResetOptionSetting()
    {
        Audio.SetMute = false;
        Audio.MusicVolume = 1.0f;
        Audio.SoundVolume = 1.0f;

        Screen.SetWindowed = false;
        Screen.ScreenWidth = 1920;
        Screen.ScreenHeight = 1080;

        Inputkeys.MouseSensitivity = 1.0f;
    }

    /// <summary>
    /// Require GlobalEnums.OptionStatus
    /// </summary>
    public void ChangeOptionStatus(GlobalEnums.OptionStatus status)
    {
        optionStatus = status;
    }
}
