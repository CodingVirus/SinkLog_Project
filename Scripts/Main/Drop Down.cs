
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // Dropdown UI ��Ҹ� Unity���� �����ؾ� �մϴ�.

    // �ػ� �ɼ� ����Ʈ
    private int[] resolutionsWidth = { 640, 800, 1280, 1360, 1440, 1600, 1920 };
    private int[] resolutionsHeight = { 480, 600, 720, 768, 900, 900, 1080 };

    void Start()
    {
        // Dropdown UI ��Ҹ� �ʱ�ȭ�մϴ�.
        resolutionDropdown.ClearOptions();

        // Dropdown�� ��� ������ �ػ� �ɼ��� �߰��մϴ�.
        for (int i = 0; i < resolutionsWidth.Length; i++)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutionsWidth[i] + "x" + resolutionsHeight[i]));
        }

        // Dropdown�� ǥ���մϴ�.
        resolutionDropdown.RefreshShownValue();

        // ���� �ػ󵵸� 1920x1080���� �����մϴ�.
        SetInitialResolution();
    }

    // ���� �ػ󵵸� 1920x1080���� �����ϴ� �޼���
    void SetInitialResolution()
    {
        int initialWidth = 1920;
        int initialHeight = 1080;

        for (int i = 0; i < resolutionsWidth.Length; i++)
        {
            if (resolutionsWidth[i] == initialWidth && resolutionsHeight[i] == initialHeight)
            {
                // ������ �ػ󵵷� ����
                Screen.SetResolution(initialWidth, initialHeight, Screen.fullScreen);
                resolutionDropdown.value = i; // Dropdown�� �ʱ� �������� ����
                break;
            }
        }
    }

    // Dropdown���� ������ �ػ��� �ε����� �������� �Լ�
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
        return 0; // �⺻������ ù ��° �ػ󵵸� �����մϴ�.
    }

    // Dropdown���� �ػ󵵰� ����� �� ȣ��Ǵ� �޼���
    public void OnResolutionChanged(int index)
    {
        if (index >= 0 && index < resolutionsWidth.Length)
        {
            int selectedWidth = resolutionsWidth[index];
            int selectedHeight = resolutionsHeight[index];

            // ������ �ػ󵵷� ����
            Screen.SetResolution(selectedWidth, selectedHeight, Screen.fullScreen);
        }
    }
}