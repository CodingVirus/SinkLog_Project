using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    public Toggle fullscreenToggle = null;

    void Start()
    {
        // 토글의 초기 상태를 "전체화면"으로 설정합니다.
        fullscreenToggle.isOn = true;

        // 시작할 때 전체화면 모드로 설정합니다.
        Screen.fullScreen = true;
    }
    // 토글의 값이 변경될 때 호출되는 메서드
    public void OnFullscreenToggleChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen; // 전체화면 모드 설정
    }
}
