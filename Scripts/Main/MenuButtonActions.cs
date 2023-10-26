using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GlobalEnums;


public class MenuButtonActions : MonoBehaviour
{
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject controlSetUI;
    /*
     * [TopMenu]
     *      continue
     *  MenuButtonActions : IngameMenuUnpauseGame(PauseMenuBundle)
     *  MenuButtonActions : MenuChangeStatus(0)
     *  
     *      option
     *  Menu Set SetActive : true
     *  MenuButtonActions : MenuChangeStatus(2)
     *      
     *      town
     *  FadeEffectGenerator : FadeInToTown
     *  
     *      main
     *  FadeEffectGenerator : FadeInToMain
     *      
     *      exit
     *  MenuButtonActions : MainMenuExitGame
     *      
     * 
     * [ControlSet]
     *      cancel 
     *  ControlSet SetActive : false 
     *  MenuButtonActions : MainMenuLoadKeySet
     *  MenuButtonActions : MenuChangeStatus(2)
     *      save
     *  MenuButtonActions : MainMenuSaveKeySet
     *  
     *      reset
     *  MenuButtonActions : MainMenuResetKeys
     *  OptionKey : UpdateButtonText
     *  
     *  
     * [OptionSet]
     *      KeySettingMenu
     *  Control Set SetActive : true
     *  MenuButtonActions : MenuChangeStatus(3)
     *  MenuButtonActions : MainMenuLoadKeySet
     *  OptionKey : UpdateButtonText
     *      
     *      reset
     *  MenuButtonActions
     *  Menu Set : ResetOptions
     *  MenuButtonActions : MainMenuResetOptions
     *  
     *      cancel
     *  Menu Set SetActive : false
     *  MenuButtonActions : MenuChangeStatus(1)
     *  MenuButtonActions : MainMenuLoadOption
     *      
     *      save
     *  MenuButtonActions : MainMenuSaveOptions
     *  
     */

    private static MenuButtonActions instance;
    public static MenuButtonActions Instance { get => instance; }

    private void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        optionUI.SetActive(false);
        controlSetUI.SetActive(false);
    }

    public void MainMenuStartGame()
    {
        if(!FadeEffectManager.Instance.IsFading)
        {
            FadeEffectManager.Instance.MapFadeIn(Vector3.zero, GlobalEnums.SceneName.Town);
        }
    }

    public void MainMenuToTheMain()
    {
        if (!FadeEffectManager.Instance.IsFading)
        {
            FadeEffectManager.Instance.MapFadeIn(Vector3.zero, GlobalEnums.SceneName.MainMenu);
        }
    }

    public void MainMenuExitGame() => Application.Quit();

    public void MainMenuResetKeys() => SettingsManager.Instance.ResetKeySetting();

    public void MainMenuResetOptions() => SettingsManager.Instance.ResetOptionSetting();

    public void MainMenuSaveOptions() => SettingsManager.Instance.SaveOptionSetting();

    public void MainMenuSaveKeySet() => SettingsManager.Instance.SaveKeySetting();

    public void MainMenuLoadOption() => SettingsManager.Instance.LoadOptionSetting();

    public void MainMenuLoadKeySet() => SettingsManager.Instance.LoadKeySetting();

    public void IngameMenuUnpauseGame(GameObject pauseBundle)
    {
        pauseBundle.SetActive(false);
        Time.timeScale = 1.0f;
        MenuChangeStatus(0);
    }

    public void MenuChangeStatus(int statusNum)
    {
        SettingsManager.Instance.ChangeOptionStatus((OptionStatus)statusNum);
    }

}
