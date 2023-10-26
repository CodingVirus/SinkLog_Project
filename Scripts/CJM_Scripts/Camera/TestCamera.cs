using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public GameObject PlayerObj;
    private Vector3 nowPosition;
    private float speed = 10.0f;
    private void Start()
    {
    }

    private void Update()
    {
        if (PlayerObj == null)
        {
            PlayerObj = RoomManager.Instance.PlayerObj;
        }
        if (PlayerObj != null)
        {
            transform.position = Vector3.Lerp(transform.position, nowPosition, speed * Time.unscaledDeltaTime);
        }
        nowPosition = new Vector3(PlayerObj.transform.position.x, PlayerObj.transform.position.y, -30.0f);
    }
}
