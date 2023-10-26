using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortal : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer & playerLayer) != 0) && 
            SettingsManager.Instance.Inputkeys.GetKeyPressed(GlobalEnums.InputKeys.InteractKey))
        {
            FadeEffectManager.Instance.MapFadeIn(transform.position, GlobalEnums.SceneName.BossRoom);
        }
    }
}
