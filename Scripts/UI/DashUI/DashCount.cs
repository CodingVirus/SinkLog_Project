using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCount : MonoBehaviour
{
    const int dashSlot = 6;
    public int dashCount = dashSlot;
    bool isDashCharging = false;
    float dashChargingTime = 1.3f;
    [SerializeField] GameObject dashCountImg;

    Queue<GameObject> dashImgQueue = new Queue<GameObject>();

    IEnumerator ChargingDash()
    {
        isDashCharging = true;
        while (true)
        {
            if (dashCount >= dashSlot)
            {
                isDashCharging = false;
                break;
            }
            else
            {
                yield return new WaitForSeconds(dashChargingTime);
                var chargingDash = dashImgQueue.Dequeue();
                chargingDash.SetActive(true);
                chargingDash.transform.SetParent(transform);
                dashCount++;
            }
        }
    }
    private void Awake()
    {
        for (int i = 0; i < dashSlot; i++) 
        {
            dashImgQueue.Enqueue(dashCountImg);
            Instantiate(dashImgQueue.Dequeue(), transform);
        }
    }

    public void UseDash()
    {

        if (dashCount > 0)
        {
            dashCount--;
            var useDashImg = transform.GetChild(0).gameObject;
            useDashImg.SetActive(false);
            useDashImg.transform.SetParent(null);
            dashImgQueue.Enqueue(useDashImg);
            if (!isDashCharging)
            {
                StartCoroutine(ChargingDash());
            }
        }
    }
}
