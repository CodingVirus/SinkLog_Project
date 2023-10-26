
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIResultScreen : MonoBehaviour
{
    [SerializeField] private GameObject resultBGObj;
    [SerializeField] private GameObject resultTitle;
    [SerializeField] private GameObject resultCoinValue;
    [SerializeField] private GameObject resultCoinImage;
    [SerializeField] private GameObject resultButton;
    [SerializeField] private TMP_FontAsset fontAsset;
    [SerializeField] private Material stageclearMaterial;
    [SerializeField] private Material gameoverMaterial;
    private int coinValue;

    private void Awake()
    {
        resultButton.SetActive(false);
        resultBGObj.SetActive(false);
        resultCoinImage.SetActive(false);
        resultCoinValue.SetActive(false);
        resultTitle.SetActive(false);
    }
    public void StartShowResult(bool isClear, int coin)
    {
        coinValue = coin;
        resultTitle.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.0f, 750.0f, 0.0f);
        resultButton.GetComponent<RectTransform>().localScale = Vector3.one * 3.0f;
        resultButton.GetComponent<Image>().color = Color.clear;
        StartCoroutine(ShowResult(isClear));
    }

    private IEnumerator ShowResult(bool isClear)
    {
        var screen = SettingsManager.Instance.Screen;
        var lerpMultple = 10;
        Color targetColor;

        var coinValueTMP = resultCoinValue.GetComponent<TMPro.TMP_Text>();
        var coinImage = resultCoinImage.GetComponent<Image>();
        coinValueTMP.color = Color.clear;
        coinImage.color = Color.clear;

        var buttonTransform = resultButton.GetComponent<RectTransform>();
        var buttonImage = resultButton.GetComponent<Image>();

        // bg
        resultBGObj.SetActive(true);
        resultBGObj.GetComponent<RectTransform>().sizeDelta = new Vector2(screen.ScreenWidth * 2.0f, screen.ScreenHeight * 2.0f);
        resultBGObj.GetComponent<Image>().color = Color.clear;

        targetColor = new Color(0.0f, 0.0f, 0.0f, 0.65f);
        var bgImage = resultBGObj.GetComponent<Image>();
        while (true) 
        {
            bgImage.color = Color.Lerp(bgImage.color, targetColor, Time.deltaTime * lerpMultple);
            if (bgImage.color.a > targetColor.a - 0.05f || Input.anyKeyDown)
            {
                bgImage.color = targetColor;
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.15f);

        // title
        resultTitle.SetActive(true);
        var titleRect = resultTitle.GetComponent<RectTransform>();
        var titleTMP = resultTitle.GetComponent<TMPro.TMP_Text>();

        titleTMP.fontMaterial = (isClear) ? stageclearMaterial : gameoverMaterial;
        titleTMP.font = fontAsset;
        titleTMP.fontSize = 224;
        titleTMP.alignment = TMPro.TextAlignmentOptions.Center;

        titleTMP.color = Color.clear;
        titleTMP.text = (isClear) ? "STAGE CLEAR" : "GAME OVER";

        targetColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        titleTMP.alignment = TMPro.TextAlignmentOptions.Center;
        while (true)
        {
            titleRect.anchoredPosition = Vector3.Lerp(titleRect.anchoredPosition, new Vector3(0.0f, 275.0f, 0.0f), Time.deltaTime * lerpMultple);
            titleTMP.color = Color.Lerp(titleTMP.color, targetColor, Time.deltaTime * lerpMultple);
            if (titleTMP.color.a > targetColor.a - 0.05f || Input.anyKeyDown)
            {
                titleRect.anchoredPosition = new Vector3(0.0f, 275.0f, 0.0f);
                titleTMP.color = targetColor;
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.35f);


        // coin
        resultCoinImage.SetActive(true);
        resultCoinValue.SetActive(true);

        int[] coinNums = new int[7];
        int scoreCalcTime = 0;
        int shownumber = 0;
        for (int i = 0; i < coinNums.Length; i++)
        {
            coinNums[i] = UnityEngine.Random.Range(0, 9);
        }
        string targetCoinString = coinValue.ToString();

        while (true)
        {
            coinValueTMP.color = Color.Lerp(coinValueTMP.color, targetColor, Time.deltaTime * lerpMultple);
            coinImage.color = Color.Lerp(coinImage.color, targetColor, Time.deltaTime * lerpMultple);
            for (int i = shownumber; i < coinNums.Length; i++)
            {
                coinNums[i] += UnityEngine.Random.Range(1, 3);
                if(coinNums[i] > 9)
                {
                    coinNums[i] %= 10;
                }
            }

            if (coinValueTMP.color.a > targetColor.a - 0.05f)
            {
                coinValueTMP.color = coinImage.color = targetColor;
            }

            if(scoreCalcTime > 150 || (shownumber > 0 && scoreCalcTime > 15))
            {
                if((shownumber + targetCoinString.Length) < coinNums.Length)
                {
                    coinNums[shownumber] = 0;
                }
                else
                {
                    if (coinNums.Length > shownumber)
                    {   
                        var targetcoinPos = shownumber - (coinNums.Length - targetCoinString.Length);
                        coinNums[shownumber] = int.Parse(targetCoinString[targetcoinPos].ToString());
                    }
                }
                scoreCalcTime = 0;
                shownumber++;
            }
            scoreCalcTime++;
            coinValueTMP.text = string.Concat(coinNums);
            if (shownumber > 6)
            {
                break;
            }

            if(Input.anyKey)
            {
                coinValueTMP.color = coinImage.color = targetColor;
                for (int i = 0; i < coinNums.Length; i++)
                {
                    if ((i + targetCoinString.Length) < coinNums.Length)
                    {
                        coinNums[i] = 0;
                    }
                    else
                    {
                        var targetcoinPos = i - (coinNums.Length - targetCoinString.Length);
                        coinNums[i] = int.Parse(targetCoinString[targetcoinPos].ToString());
                    }
                }
                coinValueTMP.text = string.Concat(coinNums);
                break;
            }

            yield return null;
        }
        yield return new WaitForSeconds(0.15f);


        // button
        resultButton.SetActive(true);
        while (true) 
        {
            buttonTransform.localScale -= Vector3.one * 0.05f;
            buttonImage.color = Color.Lerp(buttonImage.color, Color.white, Time.deltaTime * lerpMultple);
            if(Input.anyKeyDown || Vector3.Distance(buttonTransform.localScale, Vector3.one) < 0.25f)
            {
                buttonTransform.localScale = Vector3.one;
                buttonImage.color = Color.white;
                break;
            }
            yield return null;
        }
    }

    
}
