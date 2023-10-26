using UnityEngine;

public class DungeonEntryCheck : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    private void Update()
    {
        transform.position = playerObj.transform.position;
        if (transform.position.y < -2.0f)
        {
            FadeEffectManager.Instance.MapFadeIn(transform.position, GlobalEnums.SceneName.SinkHole);
            Destroy(gameObject);
        }
    }
}
