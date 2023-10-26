using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    public Toggle fullscreenToggle = null;

    void Start()
    {
        // ����� �ʱ� ���¸� "��üȭ��"���� �����մϴ�.
        fullscreenToggle.isOn = true;

        // ������ �� ��üȭ�� ���� �����մϴ�.
        Screen.fullScreen = true;
    }
    // ����� ���� ����� �� ȣ��Ǵ� �޼���
    public void OnFullscreenToggleChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen; // ��üȭ�� ��� ����
    }
}
