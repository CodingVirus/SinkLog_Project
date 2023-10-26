using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionKeySet : MonoBehaviour
{
    private GlobalEnums.InputKeys myKeyType;
    [SerializeField] private GameObject keyName;
    [SerializeField] private GameObject keyA;
    [SerializeField] private GameObject keyB;
    public void Initialize(int keynum)
    {
        myKeyType = (GlobalEnums.InputKeys)keynum;
        string keyname = default;
        switch(keynum)
        {
            case 0: keyname = "오른쪽 이동"; break;
            case 1: keyname = "왼쪽 이동"; break;
            case 2: keyname = "점프"; break;
            case 3: keyname = "공격"; break;
            case 4: keyname = "대쉬"; break;
            case 5: keyname = "인벤토리"; break;
            case 6: keyname = "맵"; break;
            case 7: keyname = "1번 무기"; break;
            case 8: keyname = "2번 무기"; break;
            case 9: keyname = "상호작용"; break;
            case 10: keyname = "재장전"; break;
            case 11: keyname = "1번 스킬"; break;
            case 12: keyname = "2번 스킬"; break;
        }
        keyName.GetComponentInChildren<TMP_Text>().text = keyname;
        ButtonTextUpdate();
    }

    public void ButtonTextUpdate()
    {
        var keydictionary = SettingsManager.Instance.Inputkeys.keyDictionary;
        keyA.GetComponentInChildren<TMP_Text>().text = keydictionary.GetValueOrDefault(myKeyType)[0].ToString();
        keyB.GetComponentInChildren<TMP_Text>().text = keydictionary.GetValueOrDefault(myKeyType)[1].ToString();
    }
    public void ChangeKeyA()
    {
        if(SettingsManager.Instance.OptionStatus == GlobalEnums.OptionStatus.KeySet)
        {
            StartCoroutine(ChangingKey(0));
        }
    }
    public void ChangeKeyB()
    {
        if (SettingsManager.Instance.OptionStatus == GlobalEnums.OptionStatus.KeySet)
        {
            StartCoroutine(ChangingKey(1));
        }
    }

    private IEnumerator ChangingKey(int pos)
    {
        SettingsManager.Instance.ChangeOptionStatus(GlobalEnums.OptionStatus.KeyChanging);
        switch(pos)
        {
            case 0: keyA.GetComponentInChildren<TMP_Text>().text = " "; break;
            case 1: keyB.GetComponentInChildren<TMP_Text>().text = " "; break;
        }
        while (true) 
        {
            yield return null;
            KeyCode key = KeyCode.None;
            if (Input.anyKeyDown)
            {
                foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                {
                    if(Input.GetKeyDown(kc))
                    {
                        key = kc;
                        break;
                    }
                }
            }

            if (key == KeyCode.Escape)
            {
                break;
            }
            else if (key != KeyCode.None)
            {
                SettingsManager.Instance.Inputkeys.ChangeKey(myKeyType, key, pos);
                break;
            }
        }
        SettingsManager.Instance.ChangeOptionStatus(GlobalEnums.OptionStatus.KeySet);
        ButtonTextUpdate();
    }


}
