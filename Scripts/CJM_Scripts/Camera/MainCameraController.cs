using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject roomLimiter;
    private Camera myCam;
    private Vector3 rightUpLimit;
    private Vector3 leftDownLimit;
    private Vector3 deltaVector = new Vector3 (0.5f, 0.5f, -10.0f);
    private AudioSource myAudio;
    private void Awake()
    {
        myAudio = GetComponent<AudioSource>();
        myCam = GetComponent<Camera>();
    }
    private void Start()
    {
        if(playerObj == null)
        {
            if (FindObjectsOfType<RoomManager>().Length > 0)
            {
                playerObj = RoomManager.Instance.PlayerObj;
            }
            else
            {
               playerObj = FindObjectOfType<PlayerControl>().gameObject;
            }
        }
        if (roomLimiter != null)
        {
            if (rightUpLimit == default && leftDownLimit == default)
            {
                var limit = roomLimiter.GetComponent<PolygonCollider2D>().points;
                leftDownLimit = limit[0];
                rightUpLimit = limit[2];
            }
        }
    }

    public void SetCameraPosition(Vector3 position, Vector3 rightTop, Vector2 leftDown)
    {
        transform.parent.position = position + deltaVector;
        transform.position = playerObj.transform.position + deltaVector;
        rightUpLimit = rightTop;
        leftDownLimit = leftDown;
        transform.localPosition = LimitCameraPosition();
    }


    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, playerObj.transform.position + deltaVector, 0.15f);
        transform.localPosition = LimitCameraPosition();
    }

    private void Update()
    {
        myAudio.volume = SettingsManager.Instance.Audio.MusicVolume;
    }

    private Vector3 LimitCameraPosition()
    {
        float camScreenHeight = (2.0f * myCam.orthographicSize) / 2.0f;
        float camScreenWidth = (camScreenHeight * myCam.aspect);
        var pos = transform.localPosition;

        if (pos.x + camScreenWidth > rightUpLimit.x)
        {
            pos.x = rightUpLimit.x - camScreenWidth;
        }
        else if (pos.x - camScreenWidth < leftDownLimit.x)
        {
            pos.x = leftDownLimit.x + camScreenWidth;
        }

        if (pos.y + camScreenHeight > rightUpLimit.y)
        {
            pos.y = rightUpLimit.y - camScreenHeight;
        }
        else if (pos.y - camScreenHeight < leftDownLimit.y)
        {
            pos.y = leftDownLimit.y + camScreenHeight;
        }

        return pos;
    }
}
