using UnityEngine;

public class NextMapDoor : MonoBehaviour
{
    public LayerMask PlayerMask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((gameObject.gameObject.layer & 1 << PlayerMask) != 0)
        {
            
        }
    }
}
