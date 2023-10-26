using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ShowStat : Stat
{
    [SerializeField] private TMPro.TMP_Text ADInfo;
    [SerializeField] private TMPro.TMP_Text DEFInfo;
    [SerializeField] private TMPro.TMP_Text CriRateInfo;
    [SerializeField] private TMPro.TMP_Text CriDmgInfo;
    [SerializeField] private TMPro.TMP_Text AttSpeedInfo;
    [SerializeField] private TMPro.TMP_Text MSpeedInfo;
    [SerializeField] private TMPro.TMP_Text DodgeInfo;
    // Take MoveSpeed
    PlayerControl playerstat;
    // Take Weapon Stat
    WeaponManager wm;

    bool isOpen = false;


    private void Awake()
    {
        wm = GameObject.Find("WeaponSlot").GetComponent<WeaponManager>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
         
        }

        
        if(WeaponManager.Instance.currentWeapon != null)
        {
            ADInfo.text = wm.AttMinDmg.ToString() + "~" + wm.AttMaxDmg.ToString();
            DEFInfo.text = wm.DEF.ToString();
            CriRateInfo.text = $"{wm.CriRate + basicCriRate}";
            CriDmgInfo.text = $"{wm.CriDmg + basicCriDmg}";
            AttSpeedInfo.text = $"{wm.AttSpeed}";
            MSpeedInfo.text = $"{basicMoveSpeed + wm.MSpeed}";
            DodgeInfo.text = wm.Dodge.ToString();
        }
        else
        {
            ADInfo.text = AttMinDmg.ToString() + "~" + AttMaxDmg.ToString();
            DEFInfo.text = DEF.ToString();
            CriRateInfo.text = $"{CriRate + basicCriRate}";
            CriDmgInfo.text = $"{CriDmg + basicCriDmg}";
            AttSpeedInfo.text = $"{AttSpeed}";
            MSpeedInfo.text = $"{basicMoveSpeed}";
            DodgeInfo.text = Dodge.ToString();
        }
        
    }

    void AttDmg()
    {

    }
}
