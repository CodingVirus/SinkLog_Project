using UnityEngine;

public class IngamePauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject controlSet;
    [SerializeField] private GameObject optionSet;
    [SerializeField] private GameObject pauseMenuBundle;
    [SerializeField] private MenuButtonActions menuButtonActions;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !FadeEffectManager.Instance.IsFading)
        {
            if (!pauseMenuBundle.activeSelf)
            {
                SettingsManager.Instance.ChangeOptionStatus(GlobalEnums.OptionStatus.Menu);
                Time.timeScale = 0.0f;
                pauseMenuBundle.SetActive(true);
            }
            else
            {
                var nowSettingFSM = (int)SettingsManager.Instance.OptionStatus;
                Debug.Log(nowSettingFSM);
                if (nowSettingFSM != 4)
                {
                    if (nowSettingFSM > 2)
                    {
                        controlSet.SetActive(false);
                        SettingsManager.Instance.LoadKeySetting();
                    }
                    else if(nowSettingFSM > 1)
                    {
                        optionSet.SetActive(false);
                        SettingsManager.Instance.LoadOptionSetting();
                    }
                    else if(nowSettingFSM > 0)
                    {
                        menuButtonActions.IngameMenuUnpauseGame(pauseMenuBundle);
                    }
                    SettingsManager.Instance.ChangeOptionStatus((GlobalEnums.OptionStatus)nowSettingFSM - 1);
                }
            }
        }
    }
}
