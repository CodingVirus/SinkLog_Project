using System.Collections.Generic;
using UnityEngine;

public class InputKeys<T> where T : SettingsManager
{

    public Dictionary<GlobalEnums.InputKeys, KeyCode[]> keyDictionary = new Dictionary<GlobalEnums.InputKeys, KeyCode[]>();
    public float MouseSensitivity;

    public InputKeys()
    {   
        for(int i = 0; i < 13; i++)
        {
            keyDictionary.Add((GlobalEnums.InputKeys)i, new KeyCode[2]);
        }

    }

    /// <summary>
    /// Change the keyboard key set
    /// </summary>
    public void ChangeKey(GlobalEnums.InputKeys key, KeyCode targetKey, int pos) => keyDictionary[key][pos] = targetKey;

    public bool GetKeyPressed(GlobalEnums.InputKeys key) => 
        Input.GetKeyDown(keyDictionary.GetValueOrDefault(key)[0]) || Input.GetKeyDown(keyDictionary.GetValueOrDefault(key)[1]);
    
    public bool GetKeyReleased(GlobalEnums.InputKeys key) =>
        Input.GetKeyUp(keyDictionary.GetValueOrDefault(key)[0]) || Input.GetKeyUp(keyDictionary.GetValueOrDefault(key)[1]);

    public bool GetKey(GlobalEnums.InputKeys key) => 
        Input.GetKey(keyDictionary.GetValueOrDefault(key)[0]) || Input.GetKey(keyDictionary.GetValueOrDefault(key)[1]);
}
