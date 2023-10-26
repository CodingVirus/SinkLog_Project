using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSet : MonoBehaviour
{
    [SerializeField] private GameObject soundVolumeObj;
    [SerializeField] private GameObject musicVolumeObj;
    [SerializeField] private GameObject fullscreenObj;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private int[] resolutionsWidth = { 640, 800, 1280, 1360, 1440, 1600, 1920 };
    private int[] resolutionsHeight = { 480, 600, 720, 768, 900, 900, 1080 };

    private void Start()
    {
        resolutionDropdown.ClearOptions();

        for (int i = 0; i < resolutionsWidth.Length; i++)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutionsWidth[i] + "x" + resolutionsHeight[i]));
        }

        resolutionDropdown.RefreshShownValue();

        SetInitialResolution();
    }
    private void SetInitialResolution()
    {
        int initialWidth = (int)SettingsManager.Instance.Screen.ScreenWidth;
        int initialHeight = (int)SettingsManager.Instance.Screen.ScreenHeight;
        bool isWindowed = SettingsManager.Instance.Screen.SetWindowed;

        for (int i = 0; i < resolutionsWidth.Length; i++)
        {
            if (resolutionsWidth[i] == initialWidth && resolutionsHeight[i] == initialHeight)
            {
                Screen.SetResolution(initialWidth, initialHeight, !isWindowed);
                resolutionDropdown.value = i;
                break;
            }
        }
    }
    private void Update()
    {
        UpdateTheOptions();
    }

    private void OnEnable()
    {
        soundVolumeObj.GetComponent<Slider>().value = SettingsManager.Instance.Audio.SoundVolume;
        musicVolumeObj.GetComponent<Slider>().value = SettingsManager.Instance.Audio.MusicVolume;
        var nowScreenSet = SettingsManager.Instance.Screen;
        fullscreenObj.GetComponent<Toggle>().isOn = !nowScreenSet.SetWindowed;
        resolutionDropdown.value = 6;
        for (int i = 0; i <= resolutionsWidth.Length; i++)
        {
            if (resolutionsWidth[i] == nowScreenSet.ScreenWidth && resolutionsHeight[i] == nowScreenSet.ScreenHeight)
            {
                resolutionDropdown.value = i;
                break;
            }
        }
    }
    public void UpdateTheOptions()
    {
        SettingsManager.Instance.Audio.SoundVolume = soundVolumeObj.GetComponent<Slider>().value;
        SettingsManager.Instance.Audio.MusicVolume = musicVolumeObj.GetComponent<Slider>().value;
        var nowScreenSet = SettingsManager.Instance.Screen;
        nowScreenSet.SetWindowed = !fullscreenObj.GetComponent<Toggle>().isOn;
        if (resolutionsWidth[resolutionDropdown.value] != nowScreenSet.ScreenWidth && resolutionsHeight[resolutionDropdown.value] != nowScreenSet.ScreenHeight)
        {
            nowScreenSet.ScreenWidth = (uint)resolutionsWidth[resolutionDropdown.value];
            nowScreenSet.ScreenHeight = (uint)resolutionsHeight[resolutionDropdown.value];
        }
        Screen.SetResolution((int)nowScreenSet.ScreenWidth, (int)nowScreenSet.ScreenHeight, nowScreenSet.SetWindowed);
    }

    public void ResetOptions()
    {
        soundVolumeObj.GetComponent<Slider>().value = 1.0f;
        musicVolumeObj.GetComponent<Slider>().value = 1.0f;
        fullscreenObj.GetComponent<Toggle>().isOn = true;
        resolutionDropdown.value = 6;
    }
}
