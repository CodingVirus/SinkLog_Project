using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    public float EffectHoldTime;
    public float ScaleMultiple;

    public RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void RoomFade()
    {
        StartCoroutine(RoomFading());
    }
    private IEnumerator RoomFading()
    {
        EffectFadeIn(ScaleMultiple);
        yield return new WaitForSecondsRealtime(EffectHoldTime);
        EffectFadeOut(ScaleMultiple);
    }

    public void EffectFadeIn(float sizeMultiple)
    {
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(Vector3.one * sizeMultiple, 1.0f * EffectHoldTime).SetUpdate(true);
        StartCoroutine(KillDO(EffectHoldTime));
    }
    public void EffectFadeOut(float sizeMultiple, float dalayTime = 0)
    {
        StartCoroutine(EffectFadingOut(sizeMultiple, dalayTime));
    }
    private IEnumerator EffectFadingOut(float sizeMultiple, float dalayTime = 0)
    {
        rectTransform.localScale = Vector3.one * sizeMultiple;
        yield return new WaitForSeconds(dalayTime);
        rectTransform.DOScale(Vector3.zero, 1.0f * EffectHoldTime).SetUpdate(true);
        StartCoroutine(KillDO(EffectHoldTime * 1.5f, false));
    }



    private IEnumerator KillDO(float t, bool activeSet = true)
    {
        yield return new WaitForSecondsRealtime(t);
        rectTransform.DOKill();
        gameObject.SetActive(activeSet);
    }
}

