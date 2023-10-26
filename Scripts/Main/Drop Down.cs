
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // Dropdown UI 요소를 Unity에서 연결해야 합니다.

    // 해상도 옵션 리스트
    private int[] resolutionsWidth = { 640, 800, 1280, 1360, 1440, 1600, 1920 };
    private int[] resolutionsHeight = { 480, 600, 720, 768, 900, 900, 1080 };

    void Start()
    {
        // Dropdown UI 요소를 초기화합니다.
        resolutionDropdown.ClearOptions();

        // Dropdown에 사용 가능한 해상도 옵션을 추가합니다.
        for (int i = 0; i < resolutionsWidth.Length; i++)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutionsWidth[i] + "x" + resolutionsHeight[i]));
        }

        // Dropdown을 표시합니다.
        resolutionDropdown.RefreshShownValue();

        // 시작 해상도를 1920x1080으로 설정합니다.
        SetInitialResolution();
    }

    // 시작 해상도를 1920x1080으로 설정하는 메서드
    void SetInitialResolution()
    {
        int initialWidth = 1920;
        int initialHeight = 1080;

        for (int i = 0; i < resolutionsWidth.Length; i++)
        {
            if (resolutionsWidth[i] == initialWidth && resolutionsHeight[i] == initialHeight)
            {
                // 선택한 해상도로 변경
                Screen.SetResolution(initialWidth, initialHeight, Screen.fullScreen);
                resolutionDropdown.value = i; // Dropdown을 초기 선택으로 설정
                break;
            }
        }
    }

    // Dropdown에서 선택한 해상도의 인덱스를 가져오는 함수
    int GetCurrentResolutionIndex()
    {
        Resolution currentResolution = Screen.currentResolution;
        for (int i = 0; i < resolutionsWidth.Length; i++)
        {
            if (resolutionsWidth[i] == currentResolution.width && resolutionsHeight[i] == currentResolution.height)
            {
                return i;
            }
        }
        return 0; // 기본적으로 첫 번째 해상도를 선택합니다.
    }

    // Dropdown에서 해상도가 변경될 때 호출되는 메서드
    public void OnResolutionChanged(int index)
    {
        if (index >= 0 && index < resolutionsWidth.Length)
        {
            int selectedWidth = resolutionsWidth[index];
            int selectedHeight = resolutionsHeight[index];

            // 선택한 해상도로 변경
            Screen.SetResolution(selectedWidth, selectedHeight, Screen.fullScreen);
        }
    }
}