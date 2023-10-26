using UnityEngine;

public class RoomInit : MonoBehaviour
{
    private void Start()
    {
        FadeEffectManager.Instance.MapFadeOut();
        Destroy(gameObject);
    }
}
