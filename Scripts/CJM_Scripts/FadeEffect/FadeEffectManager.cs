using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeEffectManager : MonoBehaviour
{
    private FadeEffect[] myFadeEffects = new FadeEffect[200];
    private float effectHoldTime = 0.30f;
    [SerializeField] private FadeEffect fadeEffect;
    private RectTransform canvasRectTransform;

    private float effectWidthSize;
    private float effectHeightSize;

    private static FadeEffectManager instance;
    public static FadeEffectManager Instance { get => instance; }

    private bool isFading = false;
    public bool IsFading { get => isFading; }

    private void Awake()
    {
        instance = this;
        canvasRectTransform = GetComponentInParent<RectTransform>();
        for (int index = 0; index < myFadeEffects.Length; index++)
        {
            myFadeEffects[index] = Instantiate(fadeEffect, transform);
            myFadeEffects[index].gameObject.SetActive(false);
            myFadeEffects[index].EffectHoldTime = effectHoldTime;
        }
        effectWidthSize = myFadeEffects[0].GetComponent<Image>().sprite.texture.width;
        effectHeightSize = myFadeEffects[0].GetComponent<Image>().sprite.texture.height;
    }
    
    public void JumpToNextRoom(GameObject playerObj, Vector3 target, Vector3 jumpDir, int activeSetRoomNum, int DeactivateRoomNum)
    {
        Time.timeScale = 0.0f;
        isFading = true;
        StartCoroutine(JumpingToNextRoom(playerObj, target, jumpDir, activeSetRoomNum, DeactivateRoomNum));
    }
    private IEnumerator JumpingToNextRoom(GameObject playerObj, Vector3 target, Vector3 jumpDir, int activeSetRoomNum, int DeactivateRoomNum)
    {
        var effectSizeScale = 0.75f;
        var effectXpadding = effectWidthSize * effectSizeScale;
        var effectYpadding = effectHeightSize * effectSizeScale;

        var canvasWidth = canvasRectTransform.rect.width;
        var canvasHeight = canvasRectTransform.rect.height;

        int outerMaxLoop = Mathf.RoundToInt((canvasWidth / 2) / effectXpadding);
        int innerMaxLoop = Mathf.RoundToInt((canvasHeight / 2) /  effectYpadding);

        if(jumpDir == Vector3.left || jumpDir == Vector3.down)
        {
            outerMaxLoop = -outerMaxLoop;
            innerMaxLoop = -innerMaxLoop;
        }
        int outerLoopDir = Math.Sign(outerMaxLoop);
        int innerLoopDir = Math.Sign(innerMaxLoop);
        if (jumpDir == Vector3.down || jumpDir == Vector3.up)
        {
            outerMaxLoop ^= innerMaxLoop;
            innerMaxLoop ^= outerMaxLoop;
            outerMaxLoop ^= innerMaxLoop;
        }

        var spawnDelay = (canvasWidth >= canvasHeight) ?
            effectHoldTime / Mathf.Abs(outerMaxLoop) : effectHoldTime / Mathf.Abs(innerMaxLoop);
        spawnDelay *= effectHoldTime;

        var effectNum = 0;
        for (int outerNum = -outerMaxLoop; Math.Abs(outerNum) < Math.Abs(outerMaxLoop + outerLoopDir); outerNum += outerLoopDir)
        {
            for (int innerNum = -innerMaxLoop; Math.Abs(innerNum) < Math.Abs(innerMaxLoop + innerLoopDir); innerNum += innerLoopDir)
            {
                if (!myFadeEffects[effectNum].isActiveAndEnabled)
                {
                    if (jumpDir == Vector3.right || jumpDir == Vector3.left)
                    {
                        myFadeEffects[effectNum].gameObject.SetActive(true);
                        myFadeEffects[effectNum].rectTransform.anchoredPosition = new Vector3(outerNum * effectXpadding, innerNum * effectYpadding, 0);
                    }
                    else
                    {
                        myFadeEffects[effectNum].gameObject.SetActive(true);
                        myFadeEffects[effectNum].rectTransform.anchoredPosition = (new Vector3(innerNum * effectXpadding, outerNum * effectYpadding, 0));
                    }
                    myFadeEffects[effectNum].ScaleMultiple = effectSizeScale * 1.5f;
                    myFadeEffects[effectNum++].RoomFade();
                }
                else
                {
                    innerNum += -innerLoopDir;
                    effectNum++;
                }
            }
            yield return new WaitForSecondsRealtime(spawnDelay);
        }
        RoomManager.Instance.MoveToPosition(playerObj, target);
        RoomManager.Instance.ActivateRoom(activeSetRoomNum);
        yield return new WaitForSecondsRealtime(effectHoldTime * 2);
        
        Time.timeScale = 1.0f;
        isFading = false;
        RoomManager.Instance.DeactivateRoom(DeactivateRoomNum);
    }

    public void FadeInToTown()
    {
        if(!isFading)
        {
            StartCoroutine(MapFadingIn(Vector3.zero, GlobalEnums.SceneName.Town));
        }
    }

    public void FadeInToMain()
    {
        if (!isFading)
        {
            StartCoroutine(MapFadingIn(Vector3.zero, GlobalEnums.SceneName.MainMenu));
        }
    }


    public void MapFadeIn(GameObject targetObj, GlobalEnums.SceneName targetScene)
    {
        StartCoroutine(MapFadingIn(targetObj.transform.position, targetScene));
    }
    public void MapFadeIn(Vector3 position, GlobalEnums.SceneName targetScene)
    {
        StartCoroutine(MapFadingIn(position, targetScene));
    }

    private IEnumerator MapFadingIn(Vector3 position, GlobalEnums.SceneName targetScene)
    {
        Time.timeScale = 0;
        isFading = true;
        var effectSizeScale = 0.5f;
        var effectXpadding = effectWidthSize * effectSizeScale;
        var effectYpadding = effectHeightSize * effectSizeScale;

        var canvasWidth = canvasRectTransform.rect.width;
        var canvasHeight = canvasRectTransform.rect.height;

        var deltaNum = (canvasWidth >= canvasHeight) ?
            Mathf.RoundToInt((canvasWidth / effectXpadding) * 0.5f) : Mathf.RoundToInt((canvasHeight / effectYpadding) * 0.5f);
        
        var effectIndex = 0;

        float spawnDelay = 1.0f / deltaNum;
        while (deltaNum >= 0) 
        {
            for (int ix = -(deltaNum + 1); ix <= deltaNum + 1; ix++)
            {
                for (int iy = -(deltaNum + 1); iy <= deltaNum + 1; iy++)
                {
                    if (!myFadeEffects[effectIndex].isActiveAndEnabled)
                    {
                        if (Math.Abs(ix) == deltaNum || (Math.Abs(iy) == deltaNum))
                        {
                            var playerCanvasPos = Camera.main.WorldToViewportPoint(position);
                            var createXpos = (ix * effectXpadding) + playerCanvasPos.x; // - (effectXpadding / 2);
                            var createypos = (iy * effectYpadding) + playerCanvasPos.y; // + (effectXpadding / 2);

                            if (IsPositionInsideCanvas(createXpos, createypos, effectXpadding, effectYpadding))
                            {
                                myFadeEffects[effectIndex].gameObject.SetActive(true);
                                myFadeEffects[effectIndex].rectTransform.anchoredPosition = new Vector3(createXpos, createypos, -10.0f);
                                myFadeEffects[effectIndex].ScaleMultiple = effectSizeScale;
                                myFadeEffects[effectIndex++].EffectFadeIn(effectSizeScale * 1.25f);
                            }
                            
                        }
                    }
                }
            }
            deltaNum--;
            yield return new WaitForSecondsRealtime(spawnDelay);
        }
        yield return new WaitForSecondsRealtime(1.0f);
        Time.timeScale = 1.0f;
        isFading = false;
        SceneManager.LoadSceneAsync((int)targetScene);
    }

    public void MapFadeOut()
    {
        isFading = true;
        var effectSizeScale = 0.5f;
        var effectXpadding = effectWidthSize * effectSizeScale;
        var effectYpadding = effectHeightSize * effectSizeScale;

        var canvasWidth = canvasRectTransform.rect.width;
        var canvasHeight = canvasRectTransform.rect.height;

        var deltaNum = (canvasWidth >= canvasHeight) ?
            Mathf.RoundToInt((canvasWidth / effectXpadding) * 0.5f) : Mathf.RoundToInt((canvasHeight / effectYpadding) * 0.5f);

        var effectIndex = 0;
        var delay = deltaNum * 0.1f;

        while (deltaNum >= 0)
        {
            for (int ix = -(deltaNum + 1); ix <= deltaNum + 1; ix++)
            {
                for (int iy = -(deltaNum + 1); iy <= deltaNum + 1; iy++)
                {
                    if (!myFadeEffects[effectIndex].isActiveAndEnabled)
                    {
                        if (Math.Abs(ix) == deltaNum || (Math.Abs(iy) == deltaNum))
                        {
                            var createXpos = (ix * effectXpadding);
                            var createypos = (iy * effectYpadding);

                            if (IsPositionInsideCanvas(createXpos, createypos, effectXpadding, effectYpadding))
                            {
                                myFadeEffects[effectIndex].gameObject.SetActive(true);
                                myFadeEffects[effectIndex].rectTransform.anchoredPosition = new Vector3(createXpos, createypos, -10.0f);
                                myFadeEffects[effectIndex].ScaleMultiple = effectSizeScale;
                                myFadeEffects[effectIndex++].EffectFadeOut(effectSizeScale * 1.25f, delay);
                            }
                        }
                    }
                }
            }
            delay -= 0.1f;
            deltaNum--;
        }
        StartCoroutine(WaitingEndEffect());   
    }

    private IEnumerator WaitingEndEffect()
    {
        var isBreakable = false;
        while (true) 
        {
            isBreakable = true;
            foreach (var effect in myFadeEffects)
            {
                if (effect.gameObject.activeSelf)
                {
                    isBreakable = false;
                    break;
                }
            }
            if(isBreakable)
            {
                break;
            }
            yield return null;
        }
        isFading = false;
    }

    private bool IsPositionInsideCanvas(float ix, float iy, float xSize, float ySize)
    {
        var canvasMaxWitdh = canvasRectTransform.rect.width / 2;
        var canvasMaxHeight = canvasRectTransform.rect.height / 2;
        if (ix <= canvasMaxWitdh + xSize && ix >= -canvasMaxWitdh - xSize && iy <= canvasMaxHeight + ySize && iy >= -canvasMaxHeight - ySize)
        {
            return true;
        }   
        return false;
    }

}

