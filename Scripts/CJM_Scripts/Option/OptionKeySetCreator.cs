using UnityEngine;

public class OptionKeySetCreator : MonoBehaviour
{
    [SerializeField] private GameObject KeyButtonObj;
    private GameObject[] optionKeys = new GameObject[13];
    private void Awake()
    {
        for (int i = 0; i < optionKeys.Length; i++)
        {
            optionKeys[i] = Instantiate(KeyButtonObj, transform);
            optionKeys[i].name = $"optionKey[{i}]";
            optionKeys[i].GetComponent<OptionKeySet>().Initialize(i);
            optionKeys[i].transform.localPosition += i * (Vector3.down * 55.0f);
        }
    }

    public void UpdateButtonText()
    {
        for (int i = 0; i < optionKeys.Length; i++)
        {
            optionKeys[i].GetComponent<OptionKeySet>().ButtonTextUpdate();
        }
    }
}
